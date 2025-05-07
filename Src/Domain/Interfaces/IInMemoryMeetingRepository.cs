using Domain.ValidationObjects;
using ErrorOr;

namespace Infrastructure.Repositories;

public interface IInMemoryMeetingRepository
{
    ErrorOr<Success> Add(AttributeMeeting addition);
    ErrorOr<Success> Add(ErrorOrMeeting addition);
    ErrorOr<Success> Add(DataAnnotationMeeting addition);
    ErrorOr<Success> Add(SetMeeting addition);
    ErrorOr<Success> Add(PlainMeeting addition);
}