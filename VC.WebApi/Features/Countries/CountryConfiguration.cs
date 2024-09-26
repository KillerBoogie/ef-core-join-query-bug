using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VC.WebApi.Domain.Countries;
using VC.WebApi.Infrastructure.EFCore.Context;

namespace VC.WebApi.Features.Countrys
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> entity)
        {
            entity.ToTable("Country", VCDbContext.Schema);

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName(nameof(CountryId)).HasColumnOrder(0).HasConversion(
                v => v.Value,
                v => CountryId.CreateFromString(v).Value);

            entity.Property(e => e.Name).HasColumnName("Name")
                .HasMaxLength(CountryName.MaxLength * 10);

            entity.Property(e => e.Version).IsRowVersion().HasConversion<byte[]>();
        }
    }
}
