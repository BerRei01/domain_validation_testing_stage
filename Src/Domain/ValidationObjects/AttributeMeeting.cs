using Domain.AttributeValidation;

namespace Domain.ValidationObjects;

// NOTABLE: This approach uses smart property setter that check every modification based on the rules defined in the attribute
[SetRestrictions(typeof(AttributeValidationRules.ValidAttributeMeeting))]
public class AttributeMeeting
{
    private DateTime _takesPlaceWhen = DateTime.Now;

    public DateTime TakesPlaceWhen
    {
        get => _takesPlaceWhen;
        set => AttributeValidationHelper.ValidateAndSet(ref _takesPlaceWhen, value, this);
    }

    private bool _alreadyHappened = false;
    public bool AlreadyHappened 
    {
        get => _alreadyHappened;
        set => AttributeValidationHelper.ValidateAndSet(ref _alreadyHappened, value, this);
    }

    private uint _maxAttendees = 2;
    public uint MaxAttendees 
    {
        get => _maxAttendees;
        set => AttributeValidationHelper.ValidateAndSet(ref _maxAttendees, value, this);
    }

    // Having valid defaults that comply with the Validator's rules is mandatory with this approach otherwise
    // there can never be another valid instance be constructed since property changes evalute the entire object
    private List<uint> _attendeesUserIds = [];  
    public List<uint> AttendeesUserIds 
    {
        get => _attendeesUserIds;
        set => AttributeValidationHelper.ValidateAndSet(ref _attendeesUserIds, value, this);
    }
}