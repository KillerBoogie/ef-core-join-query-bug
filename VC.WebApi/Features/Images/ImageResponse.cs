using VC.WebApi.Shared.Responses;

namespace VC.WebApi.Features.Images
{
    public record ImageResponse(
       ImageSLResponse? SingleLanguageResponse = null,
       ImageMLResponse? MultiLanguageResponse = null
    )
    {
        public bool HasValue()
        {
            return SingleLanguageResponse is not null || MultiLanguageResponse is not null;
        }
    };
    public record ImageMLResponse
    (
        object Id,
        string Uri,
        string Name,
        Dictionary<string, string> Description,
        long? Size,
        int? Width,
        int? Height,
        DateTime Created,
        DateTime LastModified,
        ulong Version
    ) : IResponse;

    public record ImageSLResponse
    (
        object Id,
        string Uri,
        string Name,
        string? Description,
        long? Size,
        int? Width,
        int? Height,
        DateTime Created,
        DateTime LastModified,
        ulong Version
    ) : IResponse;
}
