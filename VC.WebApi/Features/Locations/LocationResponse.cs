using VC.WebApi.Domain.ImageItems;
using VC.WebApi.Shared.Addresses;
using VC.WebApi.Shared.Responses;

namespace VC.WebApi.Features.Locations
{
    public record LocationResponse(
       LocationSLResponse? SingleLanguageResponse = null,
       LocationMLResponse? MultiLanguageResponse = null
    )
    {
        public bool HasValue()
        {
            return SingleLanguageResponse is not null || MultiLanguageResponse is not null;
        }
    };
    public record LocationMLResponse(
        object Id,
        Dictionary<string, string> Name,
        List<ImageItemResponse> CoverImage,
        AddressRequest Address,
        DateTime Created,
        DateTime LastModified,
        ulong Version
        ) : IResponse;
    public record LocationSLResponse(
        object Id,
        string Name,
        List<ImageItemResponse> CoverImage,
        AddressRequest Address,
        DateTime Created,
        DateTime LastModified,
        ulong Version
        ) : IResponse;
}
