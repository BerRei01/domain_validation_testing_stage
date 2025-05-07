using System.ComponentModel.DataAnnotations;
using System.Net.Security;
using Domain.Errors;
using Domain.Extensions;
using Domain.ValidationObjects;
using ErrorOr;
using Infrastructure.Repositories;
using MediatR;

namespace Application.CreateMeeting.Implementation;

public class CreateSetMeetingRequestHandler : IRequestHandler<CreateSetMeetingRequest, ErrorOr<Success>>
{
    private readonly IInMemoryMeetingRepository _repository;

    public CreateSetMeetingRequestHandler(IInMemoryMeetingRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<Success>> Handle(CreateSetMeetingRequest request, CancellationToken cancellationToken)
    {
        Random randomizer = new Random();

        // Randomize how the request is handled
        switch (randomizer.Next(0, 3))
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
            
            default:
                return DomainError.NotImplemented;
                break;
        }
    }

    // Functional approach where the object itself is returned by the modification method, exceptions are handled by the surrounding method
    private async Task<ErrorOr<Success>> Method1(CreateSetMeetingRequest request)
    {
        Console.WriteLine("Using Method1");
       
        ErrorOr<Success> res = new SetMeeting().ToErrorOr()
            .ValidateAny(res => res.AlterTakesPlaceWhen(request.TakesPlaceWhen))
            .ValidateAny(res => res.AlterAlreadyHappened(request.AlreadyHappened))
            .ValidateAny(res => res.AlterMaxAttendees(request.MaxAttendees))
            .ValidateAny(res => res.AlterAttendeesUserIds(request.AttendeesUserIds))
            .Then(_repository.Add);

        return res;
    }

    // Also functional approach but modification methods do not return the object itself but void
    private async Task<ErrorOr<Success>> Method2(CreateSetMeetingRequest request)
    {
        Console.WriteLine("Using Method2");

        ErrorOr<Success> res = new SetMeeting().ToErrorOr()
            .ValidateAnyDo(res => res.SetTakesPlaceWhen(request.TakesPlaceWhen))
            .ValidateAnyDo(res => res.SetAlreadyHappened(request.AlreadyHappened))
            .ValidateAnyDo(res => res.SetMaxAttendees(request.MaxAttendees))
            .ValidateAnyDo(res => res.SetAttendeesUserIds(request.AttendeesUserIds))
            .Then(_repository.Add);

        return res;
    }
    
    // Try-catch block approach using modification methods
    private async Task<ErrorOr<Success>> Method3(CreateSetMeetingRequest request)
    {
        Console.WriteLine("Using Method3");

        try
        {
            var newMeeting = new SetMeeting();
            newMeeting.SetTakesPlaceWhen(request.TakesPlaceWhen);
            newMeeting.SetAlreadyHappened(request.AlreadyHappened);
            newMeeting.SetMaxAttendees(request.MaxAttendees);
            newMeeting.SetAttendeesUserIds(request.AttendeesUserIds);

            return _repository.Add(newMeeting);
        }
        catch (ValidationException v)
        {
            return v.AsErrorType(ErrorType.Validation);
        }
        catch (Exception e)
        {
            return e.AsErrorType();
        }
    }
}