using System.ComponentModel.DataAnnotations;

namespace Domain.ValidationObjects;

// NOTABLE: This approach uses old-school exceptions and dedicated modification methods to validate the domain,
// exceptions can be handled multiple ways, see CreateSetMeetingRequestHandler.cs 
public class SetMeeting
{
    public DateTime TakesPlaceWhen { get; private set; }

    public bool AlreadyHappened { get; private set; }

    public uint MaxAttendees { get; private set; }

    public List<uint> AttendeesUserIds { get; private set; }

    public void SetTakesPlaceWhen(DateTime value)
    {
        this.TakesPlaceWhen = value;
    }

    public SetMeeting AlterTakesPlaceWhen(DateTime value)
    {
        SetTakesPlaceWhen(value);
        return this;
    }

    // NOTABLE: I decided to offer both the Set... and Alter... methods even though only one is required,
    // the reason is since the AlterMethod returns the object itself it works well with
    // a functional chain approach:
    // ErrorOr<T> overallResult = obj.ToErrorOr().ValidateAny(obj => obj.Alter...).ValidateAny(obj => obj.Alter...).ValidateAny(obj => obj.Alter...).ValidateAny(obj => obj.Alter...);
    // see: CreateSetMeetingRequestHandler.cs
    public void SetAlreadyHappened(bool value)
    {
        if (value == true && TakesPlaceWhen.ToUniversalTime() > DateTime.UtcNow)
            throw new ValidationException(
                $"A meeting can only have happened when {nameof(TakesPlaceWhen)} lies in the past.");

        this.AlreadyHappened = value;
    }

    public SetMeeting AlterAlreadyHappened(bool value)
    {
        SetAlreadyHappened(value);
        return this;
    }
    
    public void SetMaxAttendees(uint value)
    {
        this.MaxAttendees = value;
    }

    public SetMeeting AlterMaxAttendees(uint value)
    {
        SetMaxAttendees(value);
        return this;
    }
    
    public void SetAttendeesUserIds(List<uint> value)
    {
        if (value.Count > MaxAttendees)
            throw new ValidationException($"You can not have more attendees than {nameof(MaxAttendees)} allows");
        
        this.AttendeesUserIds = value;
    }

    public SetMeeting AlterAttendeesUserIds(List<uint> value)
    {
        SetAttendeesUserIds(value);
        return this;
    }
    
}