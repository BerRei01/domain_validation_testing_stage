using System.ComponentModel.DataAnnotations;
using System.Net.Security;
using Application.CreateMeeting.Abstraction;
using Domain.Errors;
using Domain.Extensions;
using Domain.ValidationObjects;
using ErrorOr;
using Infrastructure.Repositories;
using MediatR;

namespace Application.CreateMeeting.Implementation;

public class CreateValueMeetingRequestHandler : IRequestHandler<CreateValueMeetingRequest, ErrorOr<Success>>
{
    private readonly IInMemoryMeetingRepository _repository;

    public CreateValueMeetingRequestHandler(IInMemoryMeetingRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<Success>> Handle(CreateValueMeetingRequest request, CancellationToken cancellationToken)
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

    // Functional approach where the ValueMeeting as type gets passed through the chain - clunky, but catches exceptions
    private async Task<ErrorOr<Success>> Method1(CreateValueMeetingRequest request)
    {
        Console.WriteLine("Using Method1");

        var plainMeeting = request.ToDomain();
        
        ErrorOr<Success> res = new ValueMeeting().ToErrorOr()
            .ValidateAny(from => ValueMeeting.From(plainMeeting))
            .Then(from => _repository.Add(from.Value));

        return res;
    }

    // Functional approach where we use the newly added ErrorOr features of ValueObject - but actually
    // just a longer way to write Method 4
    private async Task<ErrorOr<Success>> Method2(CreateValueMeetingRequest request)
    {
        Console.WriteLine("Using Method2");

        var plainMeeting = request.ToDomain();
        
        ErrorOr<Success> res = Result.Success.ToErrorOr()
            .Then(from => ValueMeeting.MakeErrorOr(plainMeeting))
            .Then(_repository.Add);

        return res;
    }
    
    // Try-catch block approach using pure ValueObject logic without ErrorOr additions
    private async Task<ErrorOr<Success>> Method3(CreateValueMeetingRequest request)
    {
        Console.WriteLine("Using Method3");

        var plainMeeting = request.ToDomain();
        
        try
        {
            ValueMeeting.From(plainMeeting);

            return _repository.Add(plainMeeting);
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
   
    // Very elegant solution that leverages a lot of implicit conversions
    private async Task<ErrorOr<Success>> Method4(CreateValueMeetingRequest request)
    {
        Console.WriteLine("Using Method4");

        var plainMeeting = request.ToDomain();
        
        return ValueMeeting.TestErrorOr(plainMeeting).Then(_repository.Add); // NOTABLE: Very short syntax
    }
    
}