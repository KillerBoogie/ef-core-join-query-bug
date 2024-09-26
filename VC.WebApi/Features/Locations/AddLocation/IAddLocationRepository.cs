using VC.WebApi.Domain.Locations;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Locations.AddLocation
{
    public interface IAddLocationRepository
    {
        public Task<Result> AddLocation(Location location);
        public Task<Result> AreNamesUnique(Location location);
    }
}
