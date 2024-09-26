using System.Text.RegularExpressions;
using VC.WebApi.Shared.BaseInterfaces;
using VC.WebApi.Shared.DDD;

namespace VC.WebApi.Shared.Languages
{
    public partial class Language : ValueObject, ICreate<Language, string>
    {
        public static bool IsAcceptLanguageHeaderRFCConform(string? acceptLanguageHeader)
        {

            return acceptLanguageHeader is null || RFC9110AcceptLanguageHeader().IsMatch(acceptLanguageHeader);
        }

        [GeneratedRegex("^([a-zA-Z]{1,8}(-[a-zA-Z0-9]{1,8})*|\\*)(;q=[0-1](\\.[0-9]{0,3})?)?(,( |)([a-zA-Z]{1,8}(-[a-zA-Z0-9]{1,8})*|\\*)(;q=[0-1](\\.[0-9]{0,3})?)?)*$")]
        private static partial Regex RFC9110AcceptLanguageHeader();
    }
}

