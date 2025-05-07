namespace Domain.ValidationObjects;

// Just the anemic representation of the domain object
public class PlainMeeting
{
    public DateTime TakesPlaceWhen { get; set; }
    
    public bool AlreadyHappened { get; set; }
    
    public uint MaxAttendees { get; set; }
    
    public List<uint> AttendeesUserIds { get; set; }
}