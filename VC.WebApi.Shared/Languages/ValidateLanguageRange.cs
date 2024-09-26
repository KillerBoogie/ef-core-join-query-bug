using System.Text.RegularExpressions;
using VC.WebApi.Shared.BaseInterfaces;
using VC.WebApi.Shared.DDD;

namespace VC.WebApi.Shared.Languages
{
    public partial class Language : ValueObject, ICreate<Language, string>
    {
        public static bool IsLanguageRangeRFC4647Conform(string? languageRange)
        {

            return languageRange is null || RFC4647LanguageRange().IsMatch(languageRange);
        }

        [GeneratedRegex("^((([a-zA-Z]{1,8})(-[a-zA-Z0-9]{1,8})*)|\\*)$")]
        private static partial Regex RFC4647LanguageRange();
    }
}

