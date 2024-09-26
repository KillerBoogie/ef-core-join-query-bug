using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VC.WebApi.Domain.Images;
using VC.WebApi.Domain.Locations;
using VC.WebApi.Infrastructure.EFCore.Context;

namespace VC.WebApi.Features.Locations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> entity)
        {
            entity.ToTable("Location", VCDbContext.Schema);

            entity.Property<int>("TId").UseIdentityColumn().HasColumnOrder(0);
            entity.HasIndex("TId").IsUnique().IsClustered();

            entity.HasKey(e => e.Id).IsClustered(false);
            entity.Property(e => e.Id).HasColumnName(nameof(LocationId)).HasColumnOrder(1);

            entity.Property(e => e.Name);

            entity.OwnsMany(e => e.CoverImages, ci =>
            {
                ci.ToTable("LocationCoverImage");
                ci.WithOwner().HasForeignKey("LocationId").HasConstraintName("FK_LocationCoverImage_Location_LocationId");
                ci.HasIndex("LocationId").HasDatabaseName("IX_LocationCoverImage_LocationId");
                ci.Property<long>("TId").HasColumnOrder(0);
                ci.HasKey("TId");
                ci.Property(i => i.ImageId).HasColumnName("ImageId").IsRequired();
                ci.HasOne<Image>()
                    .WithMany()
                    .HasForeignKey(i => i.ImageId)
                    .HasConstraintName("FK_LocationCoverImage_ImageId")
                    .OnDelete(DeleteBehavior.NoAction);
                ci.Property(i => i.DisplayOrder);
                ci.Property(i => i.FocusPointX).HasPrecision(precision: 4, scale: 2);
                ci.Property(i => i.FocusPointY).HasPrecision(precision: 4, scale: 2);
            });

            entity.ComplexProperty(e => e.Address).Property(e => e.DeliveryInstruction);
            entity.ComplexProperty(e => e.Address).Property(e => e.Street);
            entity.ComplexProperty(e => e.Address).Property(e => e.StreetNumber);
            entity.ComplexProperty(e => e.Address).Property(e => e.StreetAffix);
            entity.ComplexProperty(e => e.Address).Property(e => e.ZipCode);
            entity.ComplexProperty(e => e.Address).Property(e => e.City);
            entity.ComplexProperty(e => e.Address).Property(e => e.State);
            entity.ComplexProperty(e => e.Address).Property(e => e.CountryId);
            entity.ComplexProperty(e => e.Address).Property(e => e.CountryName);

            entity.Property(e => e.Version).IsRowVersion().HasConversion<byte[]>();
        }
    }
}
