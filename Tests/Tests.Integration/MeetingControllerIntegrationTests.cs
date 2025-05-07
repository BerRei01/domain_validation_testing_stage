using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using Domain.Errors;
using Domain.ValidationObjects;
using Microsoft.AspNetCore.Http;

namespace Tests.Integration;

public class MeetingControllerIntegrationTests : IClassFixture<MyCustomWebFactory>
{
    private readonly HttpClient _client;
    private const string ControllerEndpoint = "/Meeting";

    public MeetingControllerIntegrationTests(MyCustomWebFactory factory)
    {
        LocalizedErrors.LocalizedErrorLoader.LoadAll();
        LocalizedErrors.StaticLocalizer.UiCulture = CultureInfo.GetCultureInfo("en-US"); // Enforce English error messages for this test
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData("/SetMeeting")]
    [InlineData("/ErrorOrMeeting")]
    [InlineData("/AttributeMeeting")]
    [InlineData("/ValueMeeting")]
    public async Task SuccessfullyCreate_AllMeetingTypes_ViaApi(string meetingRoute)
    {
        // Arrange
        var validBody = new PlainMeeting()
        {
            TakesPlaceWhen = DateTime.Now - TimeSpan.FromDays(1), AlreadyHappened = true, MaxAttendees = 5,
            AttendeesUserIds = [1, 2, 3]
        };
        // Act
        var response = await _client.PostAsJsonAsync(ControllerEndpoint + meetingRoute, validBody);
        // Assert
        response.EnsureSuccessStatusCode();
    }

    public static IEnumerable<object[]> RepeatTest(int times)
    {
        for (int i = 0; i < times; i++)
            yield return new object[] { "/SetMeeting", i };
        for (int i = 0; i < times; i++)
            yield return new object[] { "/ErrorOrMeeting", i };
        for (int i = 0; i < times; i++)
            yield return new object[] { "/AttributeMeeting", i };
        for (int i = 0; i < times; i++)
            yield return new object[] { "/ValueMeeting", i };
    }

    [Theory]
    [MemberData(nameof(RepeatTest), parameters: 25)] // Running multiple times for coverage since handler uses randomness to determine validation method
    public async Task MeetingRoutes_RejectFaultyData(string meetingRoute, int runNumber)
    {
        // Arrange
        var faultyBody1 =
            new PlainMeeting() // Violates Business rule a meeting in the future can not already have happened
            {
                TakesPlaceWhen = DateTime.Now + TimeSpan.FromDays(100), AlreadyHappened = true, MaxAttendees = 5,
                AttendeesUserIds = [1, 2, 3]
            };
        var faultyBody2 =
            new PlainMeeting() // Violates Business rule a meeting can not have more than MaxAttendees participants
            {
                TakesPlaceWhen = DateTime.Now + TimeSpan.FromDays(1), AlreadyHappened = false, MaxAttendees = 2,
                AttendeesUserIds = [1, 2, 3, 4]
            };
        var faultyBody3 =
            new PlainMeeting() // Violates both
            {
                TakesPlaceWhen = DateTime.Now + TimeSpan.FromDays(1), AlreadyHappened = true, MaxAttendees = 2,
                AttendeesUserIds = [1, 2, 3, 4]
            };
        // Act
        var response1 = await _client.PostAsJsonAsync(ControllerEndpoint + meetingRoute, faultyBody1);
        var response2 = await _client.PostAsJsonAsync(ControllerEndpoint + meetingRoute, faultyBody2);
        var response3 = await _client.PostAsJsonAsync(ControllerEndpoint + meetingRoute, faultyBody3);
        
        // Assert
        Assert.False(response1.IsSuccessStatusCode);
        Assert.False(response2.IsSuccessStatusCode);
        Assert.False(response3.IsSuccessStatusCode);
        
        Assert.Equal(HttpStatusCode.BadRequest, response1.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response3.StatusCode);

        var responseContent1 = await response1.Content.ReadAsStringAsync();
        var responseContent2 = await response2.Content.ReadAsStringAsync();
        var responseContent3 = await response3.Content.ReadAsStringAsync();
        Assert.Contains("in the past", responseContent1, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("more attendees", responseContent2, StringComparison.OrdinalIgnoreCase);

    }
}