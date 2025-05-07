using Domain.ValidationObjects;

namespace Application.CreateMeeting.Abstraction;

public static class CreateMeetingRequestExtensions
{
    
    // Since ValueMeeting uses PlainMeeting inside the domain this conversion method is needed
    public static PlainMeeting ToDomain(this CreateMeetingRequest request)
    {
        return new PlainMeeting()
        {
            AlreadyHappened = request.AlreadyHappened,
            AttendeesUserIds = request.AttendeesUserIds,
            MaxAttendees = request.MaxAttendees,
            TakesPlaceWhen = request.TakesPlaceWhen
        };

    }
}