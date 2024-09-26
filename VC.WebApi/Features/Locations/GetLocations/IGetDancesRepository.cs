using VC.WebApi.Shared.Languages;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Locations.GetLocations
{
    public interface IGetLocationsRepository
    {
        public Task<Result<LocationsResponse>> GetLocations(List<Language> languages);
    }
}
