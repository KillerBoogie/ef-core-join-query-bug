using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;
using System.Text.Json;
using VC.WebApi.Shared.BaseInterfaces;
using VC.WebApi.Shared.Identity;
using VC.WebApi.Shared.MultiLanguage;
using VC.WebApi.Shared.Results;
using VC.WebApi.Shared.Texts;

namespace VC.WebApi.Shared.EFCore
{
    public class SharedEFCoreConverters
    {
        public static void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {

        }

        public class JsonValueConverter<T> : ValueConverter<T, string>
        {
            public JsonValueConverter() : base(
                v => JsonSerializer.Serialize(v, default(JsonSerializerOptions)),
                v => JsonSerializer.Deserialize<T>(v, default(JsonSerializerOptions))!)
            { }
        }

        public class TextValueConverter<T> : ValueConverter<T, string> where T : Text, ICreate<T, string>
        {
            public TextValueConverter()
                : base(
                        text => text.Value,
                        str => TextEFConverter<T>.Create(str).Value
                     )
            {
            }

            private class TextEFConverter<U> where U : ICreate<U, string>
            {
                public static Result<U> Create(string value) => U.Create(value);
            }
        }

        public class GuidIdValueConverter<T> : ValueConverter<T, Guid> where T : GuidId
        {
            public GuidIdValueConverter()
                : base(
                    id => id.Value,
                    value => Create(value)
                )
            {
            }
            private static T Create(Guid value)
            {
                // Find the constructor that takes a Guid parameter
                var ctor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new[] { typeof(Guid) }, null);
                if (ctor == null)
                    throw new MissingMethodException($"No constructor found on {typeof(T)} with a Guid parameter");

                return (T)ctor.Invoke(new object[] { value });
            }
        }

        //private class AccountIdConverter : ValueConverter<AccountId, Guid>
        //{
        //    public AccountIdConverter() : base(
        //        v => v.Value,
        //        v => AccountId.CreateFromGuid(v)
        //    )
        //    { }
        //}


        public class MLRequiredConverter<T> : ValueConverter<MLRequired<T>, string>
        {
            private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = {
                    new MLJsonConverterFactory(),
                    new TextJsonConverterFactory()}
            };
            public MLRequiredConverter() : base(
                obj => JsonSerializer.Serialize(obj, _jsonSerializerOptions),
                json => JsonSerializer.Deserialize<MLRequired<T>>(json, _jsonSerializerOptions)!
            )
            { }
        }

        public class MLRequiredComparer<T> : ValueComparer<MLRequired<T>>
        {
            private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = {
                    new MLJsonConverterFactory(),
                    new TextJsonConverterFactory()}
            };

            public MLRequiredComparer() : base(
                    (c1, c2) => c1!.Equals(c2!),
                    c => c.GetHashCode(),
                    c => JsonSerializer.Deserialize<MLRequired<T>>(JsonSerializer.Serialize(c, _jsonSerializerOptions), _jsonSerializerOptions)!)
            { }
        }

        public class MLConverter<T> : ValueConverter<ML<T>, string>
        {
            private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = {
                    new MLJsonConverterFactory(),
                    new TextJsonConverterFactory()}
            };
            public MLConverter() : base(
                obj => JsonSerializer.Serialize(obj, _jsonSerializerOptions),
                json => JsonSerializer.Deserialize<ML<T>>(json, _jsonSerializerOptions)!
            )
            { }
        }

        public class MLComparer<T> : ValueComparer<ML<T>>
        {
            private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = {
                    new MLJsonConverterFactory(),
                    new TextJsonConverterFactory()}
            };
            public MLComparer() : base(
                    (c1, c2) => c1!.Equals(c2!),
                    c => c.GetHashCode(),
                    c => JsonSerializer.Deserialize<ML<T>>(JsonSerializer.Serialize(c, _jsonSerializerOptions), _jsonSerializerOptions)!)
            { }
        }
    }
}
