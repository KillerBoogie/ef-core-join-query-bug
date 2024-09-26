using VC.WebApi.Domain.ImageItems;
using VC.WebApi.Shared.Addresses;

namespace VC.WebApi.Features.Locations.AddLocation
{
    public record AddLocationRequest
    (
        Dictionary<string, string> Name,
        List<ImageItemRequest> CoverImages,
        AddressRequest Address
    );
}
