using ErrorOr;
using MediatR;

namespace Application.CreateMeeting.Abstraction;

public class CreateMeetingRequest : IRequest<ErrorOr<Success>>
{
    
    public DateTime TakesPlaceWhen { get; set; }

    public bool AlreadyHappened { get; set; }

    public uint MaxAttendees { get; set; }

    public List<uint> AttendeesUserIds { get; set; }

}