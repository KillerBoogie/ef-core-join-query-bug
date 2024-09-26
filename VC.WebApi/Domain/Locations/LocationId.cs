using VC.WebApi.Shared.Identity;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Domain.Locations
{
    public class LocationId : GuidId
    {
        public static LocationId Create()
        {
            return new LocationId();
        }
        public static LocationId CreateFromGuid(Guid guid)
        {
            return new LocationId(guid);
        }

        public static Result<LocationId> CreateFromString(string? text)
        {
            Result<Guid> result = Parse<LocationId>(text);
            return result.IsSuccess ?
                Result<LocationId>.Success(new LocationId(result.Value)) :
                Result<LocationId>.Failure(result);
        }

        private LocationId(Guid guid) : base(guid) { }
        private LocationId() : base() { }
    }
}
