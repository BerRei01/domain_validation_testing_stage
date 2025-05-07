using System.Collections.Generic;
using Domain.Errors;
using Domain.ValidationObjects;
using ErrorOr;

namespace Infrastructure.Repositories;

public class InMemoryMeetingRepository : IInMemoryMeetingRepository
{
    private List<MeetingEntity> list = [];

    public ErrorOr<Success> Add(AttributeMeeting addition)
    {
        var entity = MeetingEntity.ToDomain(addition);
        list.Add(entity);
        return Result.Success;
    }
    
    public ErrorOr<Success> Add(ErrorOrMeeting addition)
    {
        var entity = MeetingEntity.ToDomain(addition);
        list.Add(entity);
        return Result.Success;
    }
    
    public ErrorOr<Success> Add(DataAnnotationMeeting addition)
    {
        var entity = MeetingEntity.ToDomain(addition);
        list.Add(entity);
        return Result.Success;
    }
    
    public ErrorOr<Success> Add(SetMeeting addition)
    {
        var entity = MeetingEntity.ToDomain(addition);
        list.Add(entity);
        return Result.Success;
    }

    public ErrorOr<Success> Add(PlainMeeting addition)
    {
        var entity = MeetingEntity.ToDomain(addition);
        list.Add(entity);
        return Result.Success;
    }

}