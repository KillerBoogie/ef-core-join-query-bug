using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VC.WebApi.Shared.Texts;
using static VC.WebApi.Shared.EFCore.SharedEFCoreConverters;

namespace VC.WebApi.Infrastructure.EFCore.ModelBuilderExtensions
{
    public static partial class ModelBuilderExtensions
    {
        public static void ApplyTextValueConverters(this ModelBuilder modelBuilder)
        {
            // List of all EF Core objects and properties that are mapped to the db
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties()
                    .Where(p => typeof(Text).IsAssignableFrom(p.PropertyType));

                foreach (var property in properties)
                {

                    var converterType = typeof(TextValueConverter<>).MakeGenericType(property.PropertyType);

                    var converter = Activator.CreateInstance(converterType);
                    if (converter is null)
                    {
                        throw new Exception("Error creating TextValueConverter Object: " + entityType.ClrType.ToString() + " Property: " + property.ToString());
                    }

                    if (!entityType.IsOwned())
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name)
                            .HasConversion((ValueConverter)converter);
                    }
                }

                // Owned types properties
                var ownedTypes = entityType.GetNavigations();
                foreach (var ownedNavigation in ownedTypes)
                {
                    var ownedEntityType = ownedNavigation.TargetEntityType.ClrType;
                    var ownedProperties = ownedEntityType.GetProperties()
                        .Where(p => typeof(Text).IsAssignableFrom(p.PropertyType));

                    foreach (var ownedProperty in ownedProperties)
                    {
                        var converterType = typeof(TextValueConverter<>).MakeGenericType(ownedProperty.PropertyType);
                        var converter = Activator.CreateInstance(converterType);
                        if (converter is null)
                        {
                            throw new Exception("Error creating TextValueConverter Object: " + ownedEntityType.ToString() + " Property: " + ownedProperty.ToString());
                        }

                        // when objects are double nested EF Core trys to configure again and causes an error.
                        // There seems to be no API to check the modelBuilder, so the error is just skipped.
                        try
                        {
                            // Here we use OwnsOne or OwnsMany as required
                            if (ownedNavigation.IsCollection)
                            {
                                modelBuilder.Entity(entityType.ClrType)
                                    .OwnsMany(ownedEntityType, ownedNavigation.Name, b =>
                                    {
                                        b.Property(ownedProperty.Name)
                                            .HasConversion((ValueConverter)converter);
                                    });
                            }
                            else
                            {
                                modelBuilder.Entity(entityType.ClrType)
                                    .OwnsOne(ownedEntityType, ownedNavigation.Name, b =>
                                    {
                                        b.Property(ownedProperty.Name)
                                            .HasConversion((ValueConverter)converter);
                                    });
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
        }
    }
}