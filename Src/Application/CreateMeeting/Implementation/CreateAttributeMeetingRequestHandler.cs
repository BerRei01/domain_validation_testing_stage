using System.ComponentModel.DataAnnotations;
using System.Net.Security;
using Domain.Errors;
using Domain.Extensions;
using Domain.ValidationObjects;
using ErrorOr;
using Infrastructure.Repositories;
using MediatR;

namespace Application.CreateMeeting.Implementation;

public class CreateAttributeMeetingRequestHandler : IRequestHandler<CreateAttributeMeetingRequest, ErrorOr<Success>>
{
    private readonly IInMemoryMeetingRepository _repository;

    public CreateAttributeMeetingRequestHandler(IInMemoryMeetingRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<Success>> Handle(CreateAttributeMeetingRequest request, CancellationToken cancellationToken)
    {
        Random randomizer = new Random();

        // Randomize how the request is handled
        switch (randomizer.Next(0, 2))
        {
            case 0:
                return await Method1(request);
                break;
            
            case 1:
                return await Method2(request);
                break;
            
            default:
                return DomainError.NotImplemented;
                break;
        }
    }

    // Functional approach using properties which throw exceptions that are captured by the surrounding method
    private async Task<ErrorOr<Success>> Method1(CreateAttributeMeetingRequest request)
    {
        Console.WriteLine("Using Method1");
       
        ErrorOr<Success> res = new AttributeMeeting().ToErrorOr()
            .ValidateAnyDo(res => res.TakesPlaceWhen = request.TakesPlaceWhen)
            .ValidateAnyDo(res => res.AlreadyHappened = request.AlreadyHappened)
            .ValidateAnyDo(res => res.MaxAttendees = request.MaxAttendees)
            .ValidateAnyDo(res => res.AttendeesUserIds = request.AttendeesUserIds)
            .Then(_repository.Add);

        return res;
    }

    // Try-catch block approach using auto-validated properties, very subtle with less impact on readability, just need to remember to handle the exceptions 
    private async Task<ErrorOr<Success>> Method2(CreateAttributeMeetingRequest request)
    {
        Console.WriteLine("Using Method2");

        // NOTABLE: Looks very familiar, validation almost "invisible" 
        try
        {
            var newMeeting = new AttributeMeeting();
            newMeeting.TakesPlaceWhen = request.TakesPlaceWhen;
            newMeeting.AlreadyHappened = request.AlreadyHappened;
            newMeeting.MaxAttendees = request.MaxAttendees;
            newMeeting.AttendeesUserIds = request.AttendeesUserIds;

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