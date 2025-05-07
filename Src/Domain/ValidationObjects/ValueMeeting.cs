using Domain.Errors;
using Domain.Primitives;
using FluentValidation;

namespace Domain.ValidationObjects;

// NOTABLE: This approach inherits from ValueObject and only defines the ValidateMethod and acts as a wrapper type for PlainMeeting,
// when a PlainMeeting is converted to a ValueMeeting the Validate method is called and possibly exceptions are raised, see
// CreateValueMeetingRequestHandler.cs for the different ways to handle them
public class ValueMeeting : ValueObject<PlainMeeting, ValueMeeting>
{
   protected override void Validate()
   {
      if (Value.AlreadyHappened && Value.TakesPlaceWhen.ToUniversalTime() > DateTime.UtcNow)
      {
         throw new ValidationException(DomainError.Meetings.CompletedMeetingsInThePast.Description);
      }

      if (Value.MaxAttendees < Value.AttendeesUserIds.Count)
      {
         throw new ValidationException(DomainError.Meetings.TooManyAttendees.Description);
      }
      
      base.Validate();
   }
}