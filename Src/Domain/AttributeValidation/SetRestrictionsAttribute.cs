using FluentValidation;

namespace Domain.AttributeValidation;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class SetRestrictionsAttribute : Attribute
{
    public Type ValidatorType { get; }

    public SetRestrictionsAttribute(Type validatorType)
    {
        if (!typeof(IValidator).IsAssignableFrom(validatorType))
            throw new ArgumentException("ValidatorType must implement IValidator.");

        ValidatorType = validatorType;
    }

    public void Validate(object instance)
    {
        var validator = (IValidator)Activator.CreateInstance(ValidatorType);
        var context = new ValidationContext<object>(instance);
        var result = validator.Validate(context);
        if (!result.IsValid)
            throw new ValidationException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
    }
}