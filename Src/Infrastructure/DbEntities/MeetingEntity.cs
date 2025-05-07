using System;
using System.Collections.Generic;
using Domain.ValidationObjects;

namespace Infrastructure.Repositories;

public class MeetingEntity
{
    public DateTime TakesPlaceWhen { get; set; }
    public bool AlreadyHappened { get; set; }
    public uint MaxAttendees { get; set; }
    public List<uint> AttendeesUserIds { get; set; } = [];

    public static MeetingEntity ToDomain(PlainMeeting meeting)
    {
        return new MeetingEntity
        {
            TakesPlaceWhen = meeting.TakesPlaceWhen,
            AlreadyHappened = meeting.AlreadyHappened,
            MaxAttendees = meeting.MaxAttendees,
            AttendeesUserIds = meeting.AttendeesUserIds ?? []
        };
    }

    public static MeetingEntity ToDomain(DataAnnotationMeeting meeting)
    {
        return new MeetingEntity
        {
            TakesPlaceWhen = meeting.TakesPlaceWhen,
            AlreadyHappened = meeting.AlreadyHappened,
            MaxAttendees = meeting.MaxAttendees,
            AttendeesUserIds = meeting.AttendeesUserIds ?? []
        };
    }

    public static MeetingEntity ToDomain(AttributeMeeting meeting)
    {
        return new MeetingEntity
        {
            TakesPlaceWhen = meeting.TakesPlaceWhen,
            AlreadyHappened = meeting.AlreadyHappened,
            MaxAttendees = meeting.MaxAttendees,
            AttendeesUserIds = meeting.AttendeesUserIds ?? []
        };
    }

    public static MeetingEntity ToDomain(ErrorOrMeeting meeting)
    {
        return new MeetingEntity
        {
            TakesPlaceWhen = meeting.TakesPlaceWhen,
            AlreadyHappened = meeting.AlreadyHappened,
            MaxAttendees = meeting.MaxAttendees,
            AttendeesUserIds = meeting.AttendeesUserIds ?? []
        };
    }

    public static MeetingEntity ToDomain(SetMeeting meeting)
    {
        return new MeetingEntity
        {
            TakesPlaceWhen = meeting.TakesPlaceWhen,
            AlreadyHappened = meeting.AlreadyHappened,
            MaxAttendees = meeting.MaxAttendees,
            AttendeesUserIds = meeting.AttendeesUserIds ?? []
        };
    }
}