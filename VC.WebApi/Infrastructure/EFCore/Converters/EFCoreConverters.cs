using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using NodaTime.Text;
using System.Text.Json;
using VC.WebApi.Domain.Countries;
using VC.WebApi.Domain.DisplayOrders;
using VC.WebApi.Domain.ImageItems;
using VC.WebApi.Domain.Images;
using VC.WebApi.Domain.Locations;
using VC.WebApi.Shared.Languages;
using VC.WebApi.Shared.MultiLanguage;
using VC.WebApi.Shared.Quantities;
using VC.WebApi.Shared.Texts;
using static VC.WebApi.Shared.EFCore.SharedEFCoreConverters;

namespace VC.WebApi.Infrastructure.EFCore.Converters
{
    internal class EFCoreConverters
    {
        public static void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {

            configurationBuilder.Properties<MLRequired<LocationName>>()
                .HaveConversion<MLRequiredConverter<LocationName>, MLRequiredComparer<LocationName>>();

            configurationBuilder.Properties<MLRequired<CountryName>>()
                .HaveConversion<MLRequiredConverter<CountryName>, MLRequiredComparer<CountryName>>();

            configurationBuilder.Properties<MLRequired<ImageTitle>>().HaveConversion<MLRequiredConverter<ImageTitle>, MLRequiredComparer<ImageTitle>>();
            configurationBuilder.Properties<ML<ImageTitle>>().HaveConversion<MLConverter<ImageTitle>, MLComparer<ImageTitle>>();

            configurationBuilder.Properties<ML<ImageDescription>>().HaveConversion<MLConverter<ImageDescription>, MLComparer<ImageDescription>>();

            configurationBuilder.Properties<Duration>().HaveConversion<DurationToTicksConverter>();

            configurationBuilder.Properties<Period>().HaveConversion<PeriodToStringConverter>();

            configurationBuilder.Properties<DisplayOrder>().HaveConversion<DisplayOrderConverter>();

            configurationBuilder.Properties<NonZeroQuantity>().HaveConversion<NonZeroQuantityConverter>();


            configurationBuilder.Properties<ScreenSize>().HaveConversion<ScreenSizeConverter>();

            configurationBuilder.Properties<Language>().HaveConversion<LanguageConverter>();

            configurationBuilder.Properties<FocusPoint>().HaveConversion<FocusPointConverter>();

        }


        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {
                    new  TextJsonConverterFactory(),
                    new MLJsonConverterFactory()
            }
        };


        public class ScreenSizeConverter : ValueConverter<ScreenSize, string>
        {
            public ScreenSizeConverter() : base(
                screenSize => screenSize.ToString(),
                value => ScreenSize.Create(value).Value)
            {
            }
        }

        public class LanguageConverter : ValueConverter<Language, string>
        {
            public LanguageConverter() : base(
                language => language.ToString(),
                value => Language.Create(value).Value)
            {
            }
        }

        public class FocusPointConverter : ValueConverter<FocusPoint, decimal>
        {
            public FocusPointConverter() : base(
                FocusPoint => FocusPoint.Value,
                value => FocusPoint.Create(value).Value)
            {
            }
        }

        public class PeriodToStringConverter : ValueConverter<Period, string>
        {
            private static readonly PeriodPattern _periodPattern = PeriodPattern.NormalizingIso;

            public PeriodToStringConverter()
                : base(
                    period => _periodPattern.Format(period),
                    str => ParsePeriod(str))
            {
            }

            private static Period ParsePeriod(string str)
            {
                var parseResult = _periodPattern.Parse(str);
                if (parseResult.Success)
                {
                    return parseResult.Value;
                }
                throw new FormatException($"Cannot parse value '{str}' to NodaTime.Period.");
            }
        }

        public class DurationToTicksConverter : ValueConverter<Duration, long>
        {
            public DurationToTicksConverter()
                : base(
                    duration => duration.BclCompatibleTicks,
                    ticks => Duration.FromTicks(ticks))
            {
            }
        }

        public class DisplayOrderConverter : ValueConverter<DisplayOrder, int>
        {
            public DisplayOrderConverter()
                : base(
                    displayOrder => displayOrder.Value,
                    v => DisplayOrder.Create(v).Value)
            {
            }
        }

        public class QuantityConverter : ValueConverter<Quantity, int>
        {
            public QuantityConverter() : base(
                q => q.Value,
                value => Quantity.Create(value).Value
            )
            { }
        }

        public class NonZeroQuantityConverter : ValueConverter<NonZeroQuantity, int>
        {
            public NonZeroQuantityConverter() : base(
                q => q.Value,
                value => NonZeroQuantity.Create(value).Value
            )
            { }
        }


        //private class ImageListConverter : ValueConverter<IReadOnlyList<Image>, string>
        //{
        //    public ImageListConverter() : base(
        //        v => JsonSerializer.Serialize(v, jsonOptions),
        //        v => v.Equals(string.Empty) ? new List<Image>() : JsonSerializer.Deserialize<IReadOnlyList<Image>>(v, jsonOptions)!
        //    )
        //    { }
        //}

        private class TitledImageListConverter : ValueConverter<IReadOnlyList<TitledImageItem>, string>
        {
            public TitledImageListConverter() : base(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => v.Equals(string.Empty) ? new List<TitledImageItem>() : JsonSerializer.Deserialize<IReadOnlyList<TitledImageItem>>(v, jsonOptions)!
            )
            { }
        }

    }
}
