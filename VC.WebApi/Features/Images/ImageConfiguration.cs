using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VC.WebApi.Domain.Images;
using VC.WebApi.Infrastructure.EFCore.Context;
using VC.WebApi.Shared.Files;

namespace VC.WebApi.Features.Images
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> entity)
        {
            entity.ToTable("Image", VCDbContext.Schema);

            entity.Property<int>("TId").UseIdentityColumn().HasColumnOrder(0);
            entity.HasIndex("TId").IsUnique().IsClustered();

            entity.HasKey(e => e.Id).IsClustered(false);
            entity.Property(e => e.Id).HasColumnName(nameof(ImageId)).HasColumnOrder(1);

            entity.Property(e => e.FileName).HasMaxLength(FileName.MaxLength).HasColumnOrder(2);
            entity.HasIndex("FileName").IsUnique();
            entity.Property(e => e.Description).HasColumnOrder(3);

            entity.Property(e => e.Uri).HasColumnOrder(4);
            entity.HasIndex("Uri").IsUnique();
            entity.ComplexProperty(e => e.MetaData).Property(e => e.Width).HasColumnOrder(5);
            entity.ComplexProperty(e => e.MetaData).Property(e => e.Height).HasColumnOrder(6);
            entity.ComplexProperty(e => e.MetaData).Property(e => e.Size).HasColumnOrder(7);

            entity.Property(e => e.Version).IsRowVersion().HasConversion<byte[]>();
        }
    }
}
