using System.Globalization;
using System.Reflection;
using ErrorOr;

namespace Domain.Errors;

// NOTABLE: Contains the entire logic for localized ErrorOr errors like seen in DomainError.cs
public class LocalizedErrors
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class LocalizedErrorAttribute : Attribute
    {
        public string Culture { get; }
        public string Message { get; }

        public LocalizedErrorAttribute(string culture, string message)
        {
            Culture = culture;
            Message = message;
        }
    }

    public static class StaticLocalizer
    {
        public static CultureInfo UiCulture = CultureInfo.CurrentUICulture;

        private static readonly Dictionary<string, Dictionary<string, string>> _errorMessages =
            new(StringComparer.OrdinalIgnoreCase);

        public static string Get(string code)
        {
            var culture = UiCulture.Name;

            if (_errorMessages.TryGetValue(code, out var cultures) &&
                cultures.TryGetValue(culture, out var message))
            {
                return message;
            }

            return code; // fallback
        }

        public static IReadOnlyDictionary<string, Dictionary<string, string>> Err => _errorMessages;

        public static void Register(string code, string culture, string message)
        {
            if (!_errorMessages.ContainsKey(code))
                _errorMessages[code] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            _errorMessages[code][culture] = message;
        }
    }

    public static class LocalizedErrorLoader
    {
        public static void LoadErrorsFromType(Type typeWithErrors)
        {
            var fields = typeWithErrors
                .GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                var attrs = field.GetCustomAttributes<LocalizedErrorAttribute>();
                Error error;

                try
                {
                    error = (Error)field.GetValue(null);
                }
                catch (NullReferenceException e)
                {
                    continue;
                }

                foreach (var attr in attrs)
                {
                    StaticLocalizer.Register(error.Code, attr.Culture, attr.Message);
                }
            }
        }

        public static void LoadAll()
        {
            LoadErrorsFromType(typeof(DomainError.Meetings));
        }
    }
}