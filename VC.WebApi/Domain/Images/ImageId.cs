using VC.WebApi.Shared.Identity;

namespace VC.WebApi.Domain.Images
{
    public class ImageId : GuidId
    {
        public static ImageId Create()
        {
            return new ImageId();
        }
        public static ImageId CreateFromGuid(Guid guid)
        {
            return new ImageId(guid);
        }

        private ImageId(Guid guid) : base(guid) { }
        private ImageId() : base() { }
    }
}
