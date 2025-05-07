using System.Security.Cryptography;

namespace Domain.Errors;
using ErrorOr;

public static class DomainError
{
    static DomainError()
    {
        
    }
    
    public static Error NotImplemented = Error.NotFound(
        code: $"{nameof(DomainError)}.{nameof(NotImplemented)}",
        description: "The Method you are calling is not implemented yet");
    
    public static Error Validator = Error.Validation(
        code: $"{nameof(DomainError)}.{nameof(Validator)}",
        description: "Validation failed");

    public static class Meetings
    {
        
        [LocalizedErrors.LocalizedError("en-US", "To complete a meeting it must be in the past")]
        [LocalizedErrors.LocalizedError("de-DE", "Um ein Meeting abzuschließen muss es in der Vergangenheit liegen")]
        // Needs to be property since fields are populated too early so StaticLocalizer can only fetch the fallback
        public static Error CompletedMeetingsInThePast => Error.Validation(
            code: $"{nameof(DomainError)}.{nameof(Meetings)}.{nameof(CompletedMeetingsInThePast)}",
            description: LocalizedErrors.StaticLocalizer.Get($"{nameof(DomainError)}.{nameof(Meetings)}.{nameof(CompletedMeetingsInThePast)}")
            );
        
        [LocalizedErrors.LocalizedError("en-US", "You can not have more attendees than the maximum for this meeting")]
        [LocalizedErrors.LocalizedError("de-DE", "Es ist nicht möglich mehr Teilnehmer zum Meeting hinzuzufügen als das Maxium erlaubt")]
        public static Error TooManyAttendees => Error.Validation(
            code: $"{nameof(DomainError)}.{nameof(Meetings)}.{nameof(TooManyAttendees)}",
            description: LocalizedErrors.StaticLocalizer.Get($"{nameof(DomainError)}.{nameof(Meetings)}.{nameof(TooManyAttendees)}")
            );
    

        [LocalizedErrors.LocalizedError("en-US", "Meeting Creation failed")]
        [LocalizedErrors.LocalizedError("de-DE", "Das Meeting konnte nicht erstellt werden")]
        public static Error CouldNotCreate => Error.Validation(
            code: $"{nameof(DomainError)}.{nameof(Meetings)}.{nameof(CouldNotCreate)}",
            description: LocalizedErrors.StaticLocalizer.Get($"{nameof(DomainError)}.{nameof(Meetings)}.{nameof(CouldNotCreate)}")
            );
    }
}