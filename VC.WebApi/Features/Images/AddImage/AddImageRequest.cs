namespace VC.WebApi.Features.Images
{
    public record AddImageRequest
    (
        string Uri,
        string FileName,
        Dictionary<string, string>? Description,
        int? Size,
        int? Width,
        int? Height,
        decimal? FocusPointX = null,
        decimal? FocusPointY = null
    );
}
