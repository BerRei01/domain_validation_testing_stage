using FluentValidation;

namespace Application.CreateMeeting.Abstraction;

public class CreateMeetingValidator : AbstractValidator<CreateMeetingRequest>
{
    public CreateMeetingValidator()
    {
        
        // NOTABLE: We could of course enforce the business logic here too, but then we
        // would not be able to showcase how to handle errors in multiple ways, 
        // so we only check if all fields are provided and that the Meeting is not 100 years in the past
        RuleFor(r => r.TakesPlaceWhen)
            .NotNull()
            .NotEmpty()
            .WithMessage($"{nameof(CreateMeetingRequest.TakesPlaceWhen)} should be populated");
        
        RuleFor(r => r.TakesPlaceWhen)
            .GreaterThan(DateTime.Now - TimeSpan.FromDays(365 * 100))
            .WithMessage($"{nameof(CreateMeetingRequest.TakesPlaceWhen)} should not be 100 years in the past");
        
        RuleFor(r => r.AlreadyHappened)
            .NotNull()
            .NotEmpty()
            .WithMessage($"{nameof(CreateMeetingRequest.AlreadyHappened)} should be populated");
        
        RuleFor(r => r.MaxAttendees)
            .NotNull()
            .NotEmpty()
            .WithMessage($"{nameof(CreateMeetingRequest.MaxAttendees)} should be populated");
        
        RuleFor(r => r.AttendeesUserIds)
            .NotNull()
            .NotEmpty()
            .WithMessage($"{nameof(CreateMeetingRequest.AttendeesUserIds)} should be populated");
    }
}