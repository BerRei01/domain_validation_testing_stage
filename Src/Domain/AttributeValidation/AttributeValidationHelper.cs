using System.Runtime.CompilerServices;

namespace Domain.AttributeValidation;
public static class AttributeValidationHelper
{
        public static void ValidateAndSet<T>(ref T field, T value, object instance)
        {
            // Temporarily assign value
            var backup = field;
            field = value;
    
            try
            {
                // Get the class-level attribute
                var attr = (SetRestrictionsAttribute)Attribute.GetCustomAttribute(instance.GetType(), typeof(SetRestrictionsAttribute));
                attr?.Validate(instance);
            }
            catch
            {
                // Revert value if validation fails
                field = backup;
                throw;
            }
        }
}