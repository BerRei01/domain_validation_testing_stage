using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Domain.Errors;
using Domain.Extensions;
using Domain.ValidationObjects;
using ErrorOr;

namespace Tests.UnitTests;

public class ValidationObjectsTests
{
    
    [Fact]
    void SetterMeeting_ThrowsValidationException_WhenCompletedInTheFuture()
    {
        // Create Meeting in the future 
        var sut = new SetMeeting() {};
        sut.SetTakesPlaceWhen(DateTime.Now + TimeSpan.FromDays(100));

        sut.SetAlreadyHappened(false);
        var ex = Record.Exception(() => sut.SetAlreadyHappened(true));
        Assert.NotNull(ex);
        Assert.IsType<ValidationException>(ex);
        Assert.False(sut.AlreadyHappened); // domain object remains valid, always valid
    }
    
    [Fact]
     void SetterMeetingThrowingValidationException_BreaksChain()
     {
         // Create Meeting in the future 
         var sut = new SetMeeting() {};
         sut.SetTakesPlaceWhen(DateTime.Now + TimeSpan.FromDays(100));
 
         sut.SetAlreadyHappened(false);

         // Naming Proposal: .ValidateAny() .ValidateAnyDo() .ValidateExplicit() .ValidateExplicitDo()
         var res = sut.ToErrorOr<SetMeeting>()
             
             .Then(sut => sut.AlterAlreadyHappened(false)) // Does nothing
             .ValidateAny(sut => sut.AlterAlreadyHappened(true)) // Throws exception
             .Then(sut => new SetMeeting().ToErrorOr()); // Is not executed


         Assert.True(res.IsError);
         Assert.Equal(ErrorType.Validation, res.FirstError.Type);
     }

    [Fact]
    async Task LocalizedErrorMessage_WorksWithGermanSystemLanguage()
    {
        
        LocalizedErrors.LocalizedErrorLoader.LoadAll();
        LocalizedErrors.StaticLocalizer.UiCulture = CultureInfo.GetCultureInfo("de-DE");

        var err = DomainError.Meetings.CompletedMeetingsInThePast;
        
        Assert.StartsWith("Um ein", err.Description, StringComparison.Ordinal);
        Assert.Equal("de-DE", CultureInfo.CurrentUICulture.Name);
    }
    
    [Fact]
    async Task LocalizedErrorMessage_SupportsEnglish()
    {
        
        LocalizedErrors.LocalizedErrorLoader.LoadAll();
        LocalizedErrors.StaticLocalizer.UiCulture = CultureInfo.GetCultureInfo("en-US");
        var err = DomainError.Meetings.CompletedMeetingsInThePast;
        
        Assert.StartsWith("To complete", err.Description, StringComparison.Ordinal);
        Assert.Equal("en-US", LocalizedErrors.StaticLocalizer.UiCulture.Name);
    }

    [Fact]
    void AttributeValidation_Works()
    {
        LocalizedErrors.LocalizedErrorLoader.LoadAll();
        LocalizedErrors.StaticLocalizer.UiCulture = CultureInfo.GetCultureInfo("en-US");
        // Create new meeting for max 5 in the future
        // Order of object initializer assignments matter so our object remains valid with each property initializer
        var sut = new AttributeMeeting() { MaxAttendees = 5, AttendeesUserIds = [1,2,3], TakesPlaceWhen = DateTime.Now + TimeSpan.FromDays(100)};

        sut.AlreadyHappened = false; // no issue
        
        var ex = Record.Exception(() => sut.AlreadyHappened = true); // throws exception since meeting is not in the past
        Assert.NotNull(ex);
        Assert.IsType<FluentValidation.ValidationException>(ex);
        Assert.Contains("must be in the past", ex.Message);
    }

    [Fact]
    void ErrorOrMeeting_SetProperty_Uses_AlterProperty_Correctly()
    {
        // Only SetAlreadyHappened enforces business rules, so if we can get an DomainError with Alter...() then we are good
        ErrorOrMeeting sut = new ErrorOrMeeting();
        var res = sut.ToErrorOr()
            .Then(obj => obj.AlterTakesPlaceWhen(DateTime.Now + TimeSpan.FromDays(100)));
        Assert.False(res.IsError);

        res = res.Then(obj => obj.AlterMaxAttendees(5)); // No issue

        res = res.Then(obj => obj.AlterAlreadyHappened(false)); // Does nothing
        Assert.False(res.IsError);

        // Violation: Meeting in the future can not already have happened
        res = res.Then(obj => obj.AlterAlreadyHappened(true));
        Assert.True(res.IsError);
        Assert.Contains(DomainError.Meetings.CompletedMeetingsInThePast, res.Errors);
        Assert.IsType<ErrorOr<ErrorOrMeeting>>(res);
    }

    [Fact]
    void ValueMeeting_WorksAsExpected()
    {
        PlainMeeting invalid = new PlainMeeting() {AlreadyHappened = true, MaxAttendees = 5, AttendeesUserIds = [1,2,3], TakesPlaceWhen = DateTime.Now + TimeSpan.FromDays(100)};
        PlainMeeting valid = new PlainMeeting() {AlreadyHappened = false, MaxAttendees = 5, AttendeesUserIds = [1,2,3], TakesPlaceWhen = DateTime.Now + TimeSpan.FromDays(100)};
        
        Assert.False(ValueMeeting.TryFrom(invalid, out _));
        Assert.Throws<FluentValidation.ValidationException>(() => ValueMeeting.From(invalid));
        Assert.True(ValueMeeting.MakeErrorOr(invalid).IsError);

        var errorOrSuccess = invalid.ToErrorOr()
                .Then(eo => ValueMeeting.TestErrorOr(eo))
                .Then(eo => valid.ToErrorOr())
                .Then(eo => ValueMeeting.TestErrorOr(eo))
                .Then(eo => Result.Success)
            ;
        Assert.True(errorOrSuccess.IsError);
        Assert.NotEmpty(errorOrSuccess.Errors);

        var errorOrSuccess2 = Result.Success.ToErrorOr()
                .Then(res => ValueMeeting.TestErrorOrSuccess(valid))
                .Then(res => ValueMeeting.TestErrorOrSuccess(invalid))
            ;
        Assert.True(errorOrSuccess2.IsError);
        Assert.NotEmpty(errorOrSuccess2.Errors);

        var errorOrSuccess3 = Result.Success.ToErrorOr()
                .Then(res => ValueMeeting.TestErrorOrSuccess(valid))
                .Then(res => ValueMeeting.TestErrorOrSuccess(valid))
                .Then(res => ValueMeeting.TestErrorOrSuccess(valid))
            ;

        Assert.False(errorOrSuccess3.IsError);

        var errorOrSuccess4 = Result.Success.ToErrorOr()
                .ValidateAnyDo(res => ValueMeeting.From(valid))
                .ValidateAnyDo(res => ValueMeeting.From(invalid))
                .ValidateAnyDo(res => ValueMeeting.From(valid))
            ;
        Assert.True(errorOrSuccess4.IsError);
        Assert.NotEmpty(errorOrSuccess4.Errors);

    }
}