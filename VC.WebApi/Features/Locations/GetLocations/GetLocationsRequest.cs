using VC.WebApi.Shared.Languages;

namespace VC.WebApi.Features.Locations.GetLocations
{
    public record GetLocationsRequest
    (
        List<Language> PreferredLanguages
    );
}
