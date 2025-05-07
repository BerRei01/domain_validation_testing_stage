using System.ComponentModel.DataAnnotations;
using System.Net.Security;
using Domain.Errors;
using Domain.Extensions;
using Domain.ValidationObjects;
using ErrorOr;
using Infrastructure.Repositories;
using MediatR;

namespace Application.CreateMeeting.Implementation;

public class CreateErrorOrMeetingRequestHandler : IRequestHandler<CreateErrorOrMeetingRequest, ErrorOr<Success>>
{
    private readonly IInMemoryMeetingRepository _repository;

    public CreateErrorOrMeetingRequestHandler(IInMemoryMeetingRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<Success>> Handle(CreateErrorOrMeetingRequest request, CancellationToken cancellationToken)
    {
        Random randomizer = new Random();

        // Randomize how the request is handled
        switch (randomizer.Next(0, 4))
        {
            case 0:
                return await Method1(request);
                break;

            case 1:
                return await Method2(request);
                break;

            case 2:
                return await Method3(request);
                break;

            case 3:
                return await Method4(request);
                break;

            default:
                return DomainError.NotImplemented;
                break;
        }
    }

    // Functional approach using the modification methods that return ErrorOr<ClassT>
    private async Task<ErrorOr<Success>> Method1(CreateErrorOrMeetingRequest request)
    {
        Console.WriteLine("Using Method1");

        ErrorOr<Success> res = new ErrorOrMeeting().ToErrorOr()
            .Then(res => res.AlterTakesPlaceWhen(request.TakesPlaceWhen))
            .Then(res => res.AlterAlreadyHappened(request.AlreadyHappened))
            .Then(res => res.AlterMaxAttendees(request.MaxAttendees))
            .Then(res => res.AlterAttendeesUserIds(request.AttendeesUserIds))
            .Then(_repository.Add);

        return res;
    }

    // Functional approach using the modification methods that return ErrorOr<Success>
    private async Task<ErrorOr<Success>> Method2(CreateErrorOrMeetingRequest request)
    {
        Console.WriteLine("Using Method2");

        ErrorOrMeeting meeting = new ErrorOrMeeting();
            var res = new ErrorOr<Success>()
            .Then(res => meeting.SetTakesPlaceWhen(request.TakesPlaceWhen))
            .Then(res => meeting.SetAlreadyHappened(request.AlreadyHappened))
            .Then(res => meeting.SetMaxAttendees(request.MaxAttendees))
            .Then(res => meeting.SetAttendeesUserIds(request.AttendeesUserIds))
            .Then(res => _repository.Add(meeting));

        return res;
    }

    // Procedural step-by-step approach, modification method returns ErrorOr<Success>: simple, predictable, avoids nesting 
    private async Task<ErrorOr<Success>> Method3(CreateErrorOrMeetingRequest request)
    {
        Console.WriteLine("Using Method3");

        var overallResult = new ErrorOr<Success>();
        var newMeeting = new ErrorOrMeeting();
        
        overallResult = newMeeting.SetTakesPlaceWhen(request.TakesPlaceWhen);
        if (overallResult.IsError) return overallResult.WithError(DomainError.Meetings.CouldNotCreate);
        
        overallResult = newMeeting.SetAlreadyHappened(request.AlreadyHappened);
        if (overallResult.IsError) return overallResult.WithError(DomainError.Meetings.CouldNotCreate);
        
        overallResult = newMeeting.SetMaxAttendees(request.MaxAttendees);
        if (overallResult.IsError) return overallResult.WithError(DomainError.Meetings.CouldNotCreate);
        
        overallResult = newMeeting.SetAttendeesUserIds(request.AttendeesUserIds);
        if (overallResult.IsError) return overallResult.WithError(DomainError.Meetings.CouldNotCreate);

        return _repository.Add(newMeeting);
    }
    
    // A procedural approach that uses a (flattened) list to collect results, modification method returns ErrorOr<Success>, collects all issues with the request, not just the first
    private async Task<ErrorOr<Success>> Method4(CreateErrorOrMeetingRequest request)
    {
        Console.WriteLine("Using Method4");

        var overallResults = new List<ErrorOr<Success>>();
        var newMeeting = new ErrorOrMeeting();
        
        overallResults.Add(newMeeting.SetTakesPlaceWhen(request.TakesPlaceWhen));
        overallResults.Add(newMeeting.SetAlreadyHappened(request.AlreadyHappened));
        overallResults.Add(newMeeting.SetMaxAttendees(request.MaxAttendees));
        overallResults.Add(newMeeting.SetAttendeesUserIds(request.AttendeesUserIds));
        if (overallResults.Any(res => res.IsError)) return overallResults.FlattenErrors().WithError(DomainError.Meetings.CouldNotCreate);
        // NOTABLE: Method 4 differs from Method 3 that it will collect all Validation issues and not return after the first
        
        return _repository.Add(newMeeting);
    }
}