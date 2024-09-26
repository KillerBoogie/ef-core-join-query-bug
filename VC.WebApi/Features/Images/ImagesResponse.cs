namespace VC.WebApi.Features.Images
{
    // Single Language filter is done in SQL request on server
    public record ImagesResponse(
       List<ImageSLResponse>? SingleLanguageResponse = null,
       List<ImageMLResponse>? MultiLanguageResponse = null
    )
    {
        public bool HasValue()
        {
            return SingleLanguageResponse is not null || MultiLanguageResponse is not null;
        }
    };
}
