using VC.WebApi.Shared.Auditing;
using VC.WebApi.Shared.Files;
using VC.WebApi.Shared.MultiLanguage;
using VC.WebApi.Shared.Results;
using VC.WebApi.Shared.Versioning;

namespace VC.WebApi.Domain.Images
{
    /// <summary>
    /// Image is a database-entry that either refers to an internal or external image
    /// Internal images are stored on the local filesystem and the URI starts with file://
    /// whereas external images have a web-wide URI that references the www-location.
    /// It is desirable that used images are internal, to show the website independent of changes
    /// or non-availability of the external resource but for copyright-reasons this is not possible.
    /// 
    /// Internal images are identified by their filename and the URI can be constructed using the
    /// App-Media-ImageDirectory parameter appenden by the filename.
    /// External images are identified by the URI and the filename is not needed.
    /// With this thought in mind an inheritance structure is proposed:
    /// Image as abstract class and two derived classes InternalImage and ExternalImage.
    /// </summary>
    public class Image : VersionedEntity<ImageId>
    {
        public static readonly int MaxSize = 14857600; // 10 MByte
        public FileName FileName { get; private set; }
        public ML<ImageDescription> Description { get; private set; }
        public Uri Uri { get; private set; }
        public ImageMetaData MetaData { get; private set; }

        public Image(ImageId id, Uri uri, FileName fileName, ML<ImageDescription> description, ImageMetaData metaData, CreatedInfo createdInfo) : base(id, createdInfo)
        {
            Uri = uri;
            FileName = fileName;
            Description = description;
            MetaData = metaData;
        }

#pragma warning disable CS8618
        protected Image() { }
#pragma warning restore CS8618

        public static Result<Image> Create(ImageId id, Uri uri, FileName fileName, ML<ImageDescription> description, ImageMetaData metaData, CreatedInfo createdInfo)
        {
            return Result<Image>.Success(new Image(id, uri, fileName, description, metaData, createdInfo));
        }
    }
}
