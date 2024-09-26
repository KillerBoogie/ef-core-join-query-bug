using VC.WebApi.Domain.ImageItems;
using VC.WebApi.Shared.Addresses;
using VC.WebApi.Shared.Auditing;
using VC.WebApi.Shared.MultiLanguage;
using VC.WebApi.Shared.Results;
using VC.WebApi.Shared.Versioning;

namespace VC.WebApi.Domain.Locations
{
    public class Location : VersionedEntity<LocationId>
    {
        public MLRequired<LocationName> Name { get; private set; }
        public IReadOnlyList<ImageItem> CoverImages => _coverImages.AsReadOnly();
        private List<ImageItem> _coverImages;
        public Address Address { get; private set; }

        private Location(LocationId id,
             MLRequired<LocationName> name,
             List<ImageItem> coverImages,
             Address address,
             CreatedInfo createdInfo) : base(id, createdInfo)
        {
            Name = name;
            _coverImages = coverImages;
            Address = address;
        }

#pragma warning disable CS8618 
        private Location() { }
#pragma warning restore CS8618 

        public static Result<Location> Create(LocationId LocationId,
            MLRequired<LocationName> name, Address address,
            List<ImageItem> coverImages, CreatedInfo createdInfo)
        {
            Result result = Validate();
            if (result.IsFailure)
                return Result<Location>.Failure(result);

            Location Location = new(LocationId, name, coverImages,
                address, createdInfo);

            return Result<Location>.Success(Location);
        }

        public static Result Validate()
        {
            return Result.Success();
        }
    }
}
