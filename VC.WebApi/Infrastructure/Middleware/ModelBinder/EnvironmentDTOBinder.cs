using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Net.Http.Headers;
using Serilog;
using VC.WebApi.Shared.Accounts;
using VC.WebApi.Shared.Languages;

namespace VC.WebApi.Infrastructure.Middleware.ModelBinder
{
    public partial class EnvironmentDTOBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            //PreferredLanguages
            string? acceptLanguageHeader = bindingContext.HttpContext.Request.Headers["accept-language"];


            //limit header length

            const int MaxAcceptLanguageHeaderLength = 50;

            if (acceptLanguageHeader is not null && acceptLanguageHeader.Length > MaxAcceptLanguageHeaderLength)
            {
                Log.Error(nameof(bindingContext), $@"Maximum accepted length of ""Accept-Language"" header is {MaxAcceptLanguageHeaderLength} characters!");
                bindingContext.Result = ModelBindingResult.Success(null);
            }

            if (!Language.IsAcceptLanguageHeaderRFCConform(acceptLanguageHeader))
            {
                Log.Error(nameof(bindingContext), @"""Accept-Language"" header does not conform to RFC 9110!");
                bindingContext.Result = ModelBindingResult.Success(null);
            }

            var langValues = bindingContext.HttpContext.Request.GetTypedHeaders().AcceptLanguage;
            List<string> sortedLanguages = Language.ConvertToSortedLanguageTags(langValues);

            List<Language> conformedLanguages = Language.ConformLanguageRanges(sortedLanguages);

            if (conformedLanguages.Count != sortedLanguages.Count)
            {
                Log.Error("Some or all Language-Ranges in {Accept-Language} header are not valid by RFC 4647!", acceptLanguageHeader);
                // how to return error?
            }

            //TODO JWT Use actor from JWT
            AccountId actor = AccountId.Anonymous();
            AccountId actingInNameOf = AccountId.Anonymous();

            // Save Actor in HttpContext for logging, etc.
            bindingContext.HttpContext.Items["Actor"] = actor;
            bindingContext.HttpContext.Items["AcingInNameOf"] = actingInNameOf;

            EnvironmentDTO environmentDTO =
                new EnvironmentDTO(DateTime.Now,
                    bindingContext.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    bindingContext.HttpContext.Request.Method.ToString(),
                    bindingContext.HttpContext.Request.Path,
                    bindingContext.HttpContext.Request.IsHttps,
                    actor,
                    actingInNameOf,
                    conformedLanguages,
                    true);

            bindingContext.Result = ModelBindingResult.Success(environmentDTO);

            return Task.CompletedTask;
        }

        public static List<StringWithQualityHeaderValue> ParseLanguageQueryParameter(string query)
        {
            var languages = new List<StringWithQualityHeaderValue>();

            var languageTags = query.Split(',');

            foreach (var languageTag in languageTags)
            {
                if (StringWithQualityHeaderValue.TryParse(languageTag, out var stringWithQuality))
                {
                    languages.Add(stringWithQuality);
                }
            }

            return languages;
        }
    }
}
