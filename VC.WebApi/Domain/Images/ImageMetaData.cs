using VC.WebApi.Shared.Results;

namespace VC.WebApi.Domain.Images
{
    public record ImageMetaData
    {
        public int? Width { get; private set; }
        public int? Height { get; private set; }
        public int? Size { get; private set; }

        private ImageMetaData(int? size, int? width, int? height)
        {
            Size = size;
            Width = width;
            Height = height;
        }

        //use result so that later added properties can be validated, e.g. resolution
        public static Result<ImageMetaData> Create(int? size = null, int? width = null, int? height = null)
        {
            return Result<ImageMetaData>.Success(new ImageMetaData(size, width, height));
        }
    }
}
