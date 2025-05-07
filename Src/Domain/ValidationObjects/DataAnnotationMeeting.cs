using System.ComponentModel.DataAnnotations;

namespace Domain.ValidationObjects;

// This one was intended to use DataAnnotations in conjunction with https://github.com/mind-ra/DependentValidation/tree/main
// and a similar property based always valid strategy like AttributeMeeting.cs
// but it seems the logic of the Meeting can not properly modelled even with the DataAnnotation extension
public class DataAnnotationMeeting
{
    [Required]
    public DateTime TakesPlaceWhen { get; set; }
    
    public bool AlreadyHappened { get; set; }
    
    public uint MaxAttendees { get; set; }
    
    public List<uint> AttendeesUserIds { get; set; }
}