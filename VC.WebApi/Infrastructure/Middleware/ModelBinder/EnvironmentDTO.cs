using Microsoft.AspNetCore.Mvc;
using VC.WebApi.Shared.Accounts;
using VC.WebApi.Shared.Languages;


namespace VC.WebApi.Infrastructure.Middleware.ModelBinder
{
    public record EnvironmentDTO
    {
        [FromServices] public DateTime Time { get; }
        [FromServices] public string? IpAddress { get; }
        [FromServices] public string HttpMethod { get; }
        [FromServices] public string Route { get; }
        [FromServices] public bool IsHttps { get; }
        [FromServices] public AccountId Actor { get; }
        [FromServices] public AccountId ActingInNameOf { get; }
        [FromHeader(Name = "Accept-Language")] public List<Language> PreferredLanguages { get; }
        [FromServices] public bool AllLanguages { get; }

        public EnvironmentDTO(DateTime time, string? ipAddress, string httpMethod, string route, bool isHttps, AccountId actor, AccountId actingInNameOf, List<Language> preferredLanguages, bool allLanguages)
        {
            Time = time;
            IpAddress = ipAddress;
            HttpMethod = httpMethod;
            Route = route;
            IsHttps = isHttps;
            Actor = actor;
            ActingInNameOf = actingInNameOf;
            PreferredLanguages = preferredLanguages;
            AllLanguages = allLanguages;
        }
    }
}