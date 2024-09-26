using Microsoft.EntityFrameworkCore;
using VC.WebApi.Domain.Countries;
using VC.WebApi.Domain.ImageItems;
using VC.WebApi.Domain.Images;
using VC.WebApi.Domain.Locations;
using VC.WebApi.Features.Countrys;
using VC.WebApi.Features.Images;
using VC.WebApi.Features.Locations;
using VC.WebApi.Infrastructure.EFCore.Converters;
using VC.WebApi.Infrastructure.EFCore.ModelBuilderExtensions;

namespace VC.WebApi.Infrastructure.EFCore.Context
{
    public class VCDbContext : DbContext
    {
        public const string Schema = "VC";

        public DbSet<Location> Location { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Image> Image { get; set; }
        public string? GetLanguage(object json, string preferredLanguages)
        => throw new NotSupportedException($"{nameof(GetLanguage)} cannot be called client side");
        //public string GetLanguageRequired(object json, string preferredLanguages)
        //    => throw new NotSupportedException($"{nameof(GetLanguageRequired)} cannot be called client side");

        public VCDbContext(DbContextOptions<VCDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ImageConfiguration());

            // EventLocation/-Dance/-Teacher must be ignored, because they are configured as OwnsMany in EventConfiguration
            // special EF-Core configurations for his objects like FK must be part of the OwnsMany-Configuration
            modelBuilder.Ignore<ImageItem>();
            modelBuilder.Ignore<TitledImageItem>();

            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new LocationConfiguration());

            modelBuilder.HasDbFunction(typeof(VCDbContext).GetMethod(nameof(GetLanguage), [typeof(object), typeof(string)])!,
                builder =>
                {
                    builder.HasParameter("json").HasStoreType("nvarchar(max)");
                    builder.HasParameter("preferredLanguages");
                });

            modelBuilder.ApplyGuidIdValueConverters();
            modelBuilder.ApplyTextValueConverters();

        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            EFCoreConverters.ConfigureConventions(configurationBuilder);
            //SharedEFCoreConverters.ConfigureConventions(configurationBuilder);
        }
    }
}
