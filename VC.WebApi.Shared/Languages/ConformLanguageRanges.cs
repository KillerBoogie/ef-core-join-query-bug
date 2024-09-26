using Microsoft.Net.Http.Headers;
using System.Globalization;
using VC.WebApi.Shared.BaseInterfaces;
using VC.WebApi.Shared.DDD;

namespace VC.WebApi.Shared.Languages
{
    public partial class Language : ValueObject, ICreate<Language, string>
    {
        private static readonly Dictionary<string, string> _allLanguageTags = CultureInfo.GetCultures(CultureTypes.AllCultures).Select(x => x.Name).ToDictionary(c => c, StringComparer.OrdinalIgnoreCase);
        public static List<Language> ConformLanguageRanges(IEnumerable<string> languageRanges)
        {

            List<Language> conformedLanguages = [];

            foreach (string languageRange in languageRanges)
            {
                _allLanguageTags.TryGetValue(languageRange, out string? foundLanguage);
                if (foundLanguage is not null)
                {
                    //use correct formatting from CultureInfo

                    conformedLanguages.Add(new Language(foundLanguage));
                }

            }

            return conformedLanguages;
        }

        public static List<string> ConvertToSortedLanguageTags(IList<StringWithQualityHeaderValue> acceptLanguageHeader)
        {
            //order languages by quality with stable LINQ sorting algorithm 
            //the framework sets the quality value for a string without provided "q=" to NULL. According to RFC 9110 it must be conformed to q=1
            //convert to lower case to remove duplicates with distinct
            List<string> acceptLanguagesSorted = acceptLanguageHeader.OrderByDescending(x => x.Quality is null ? 1 : x.Quality).Select(x => x.Value.ToString().ToLower()).Distinct().ToList();

            return acceptLanguagesSorted;
        }

    }
}
