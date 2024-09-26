using System.Data;
using System.Globalization;
using System.Resources;
using System.Text.RegularExpressions;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Languages;

namespace VC.WebApi.Infrastructure.Errors
{
    public static class ErrorToProblemDetail
    {

        public static ProblemDetail ToProblemDetail(this Error error)
        {
            return error.ToProblemDetail(Language.CreateFromCultureInfo(CultureInfo.CurrentUICulture));
        }

        public static ProblemDetail ToProblemDetail(this Error error, Language language)
        {
            List<Error> errors = [error];

            return errors.ToProblemDetail(language);
        }

        public static ProblemDetail ToProblemDetail(this List<Error> errors)
        {
            return errors.ToProblemDetail(Language.CreateFromCultureInfo(CultureInfo.CurrentUICulture));
        }

        public static ProblemDetail ToProblemDetail(this List<Error> errors, Language language)
        {
            if (errors.Count == 0) throw new ArgumentException("The argument errors of type List<Errors> in the ToDTO() extension is empty.");

            // collected error types
            List<int> errorTypes = [];

            foreach (Error error in errors)
            {
                int httpStatus = (int)error.Class;

                if (!errorTypes.Contains(httpStatus)) // Check for duplicates
                {
                    errorTypes.Add(httpStatus);
                }
            }

            // determine main error
            int status;
            string type;
            if (errorTypes.Count == 1)
            {
                status = errorTypes[0];
                type = errors[0].Class.ToString();
            }
            else
            {
                bool has400Values = errorTypes.Any(n => n >= 400 && n < 500);
                bool has500Values = errorTypes.Any(n => n >= 500);

                if (has500Values)
                {
                    status = 500;
                    type = ErrorClass.InternalServerError.ToString();

                    // remove 400 errors
                    errors.RemoveAll(item => (int)item.Class < 500);
                }
                else
                {
                    status = 400;
                    type = ErrorClass.BadRequest.ToString();
                }
            }

            type = PascalToKebab(type);


            // create ErrorDetails
            List<ErrorsDetail> errorDetails = [];

            Type resourceType = typeof(Field);
            ResourceManager rm = new(resourceType);
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(language.ToString());

            foreach (Error error in errors)
            {
                ErrorsDetail errorsDetail = CreateErrorsDetail(error, cultureInfo, rm);
                errorDetails.Add(errorsDetail);
            }

            string title = GetLocalizedTitle(type, cultureInfo);
            //string type = new Uri("/API/Errors/" + ConvertToUriStyle(code), UriKind.Relative).ToString();

            string traceId = System.Diagnostics.Activity.Current?.Id ?? "null";

            ProblemDetail problemDetail = new(
                type,
                title,
                status,
                traceId,
                new Dictionary<string, object?> { { "errors", errorDetails } }
                );

            return problemDetail;
        }


        private static ErrorsDetail CreateErrorsDetail(Error error, CultureInfo cultureInfo, ResourceManager rm)
        {
            Dictionary<string, object?> detailParameters = [];

            // translate fieldnames
            foreach (KeyValuePair<string, object?> p in error.DetailParameters)
            {
                if (p.Key.StartsWith("fieldName"))
                {
                    if (p.Value is null)
                    {
                        throw new NoNullAllowedException($"FillMessageParameter: The value of 'fieldName' is NULL");
                    }

                    if (p.Value is not String)
                    {
                        throw new InvalidCastException($"FillMessageParameter: 'fieldName' is not of type 'String'");
                    }

                    string value = rm.GetString((string)p.Value, cultureInfo) ?? throw new ArgumentNullException($"Key '{p.Value}' doesn't exist for language '{cultureInfo}' in 'Field.resx' resource file.");

                    detailParameters.Add(p.Key, value);
                }
                else
                {
                    detailParameters.Add(p.Key, p.Value);
                }
            }

            string detail = GetLocalizedDetail(error.Code, detailParameters, cultureInfo);
            string? pointer = error.Pointer is not null ? "#/" + error.Pointer : null;

            ErrorsDetail errorsDetail = new(
                    type: error.Code,
                    pointer: pointer,
                    detail,
                    detailParameters: detailParameters,
                    extensions: error.Extensions
                    );

            return errorsDetail;
        }

        private static string ConvertToUriStyle(string title)
        {
            string uri = title.Replace(".", "/").Replace("_", "-").ToLower();
            return uri;
        }

        public static string GetLocalizedTitle(string code, CultureInfo cultureInfo)
        {
            Type resourceType = typeof(ErrorTitle);
            ResourceManager rm = new(resourceType);

            string localized = rm.GetString(code, cultureInfo) ?? throw new ArgumentNullException($"Key '{code}' doesn't exist for language '{cultureInfo.ToString()}' in error resource file '{resourceType}'.");

            return localized;
        }

        public static string GetLocalizedDetail(string code, IDictionary<string, object?> detailParameters, CultureInfo cultureInfo)
        {
            Type resourceType = typeof(ErrorDetail);
            ResourceManager rm = new(resourceType);

            string localized = rm.GetString(code, cultureInfo) ?? throw new ArgumentNullException($"Key '{code}' doesn't exist  for language '{cultureInfo.ToString()}' in error resource file '{resourceType}'.");

            return FillMessageParameters(localized, detailParameters, cultureInfo);
        }

        private static string FillMessageParameters(string messageTemplate, IDictionary<string, object?> detailParameters, CultureInfo cultureInfo)
        {
            List<string?> values = [];
            string? value;

            foreach (KeyValuePair<string, object?> p in detailParameters)
            {
                value = p.Value is null ? "null" : p.Value.ToString();

                values.Add(value);
            }

            return string.Format(messageTemplate, values.ToArray());
        }

        static string PascalToKebab(string input)
        {
            // Insert a hyphen before each uppercase letter (except for the first one)
            string kebab = Regex.Replace(input, "(?<!^)([A-Z])", "-$1");

            // Convert the string to lowercase
            return kebab.ToLower();
        }

        //static int GetPlaceholderCount(string formatString)
        //{
        //    // Regular expression to match placeholders of the format {0}, {1}, etc.
        //    string pattern = @"\{[0-9]+\}";

        //    // Find all matches in the format string
        //    MatchCollection matches = Regex.Matches(formatString, pattern);

        //    // Return the number of unique placeholders
        //    return matches.Count;
        //}
    }
}

