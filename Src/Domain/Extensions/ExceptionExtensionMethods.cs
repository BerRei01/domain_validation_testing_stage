using ErrorOr;

namespace Domain.Extensions;

public static class ExceptionExtensionMethods
{
    public static Error AsErrorType(this Exception ex, ErrorType type = ErrorType.Unexpected)
    {
        return Error.Custom((int)type, ex.GetType().Name, ex.Message, ex.Data as Dictionary<string, object>);
    }
}