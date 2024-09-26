using System.Text.Json;
using System.Text.Json.Serialization;
using VC.WebApi.Shared.BaseInterfaces;

namespace VC.WebApi.Shared.Texts
{

    // Custom JsonConverter for Text and derived types
    public class TextJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(Text).IsAssignableFrom(typeToConvert);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var converterType = typeof(TextJsonConverter<>).MakeGenericType(typeToConvert);
            var converter = Activator.CreateInstance(converterType) as JsonConverter ?? throw new InvalidOperationException($"Unable to create an instance of {converterType}");
            return converter;
        }
    }
    public class TextJsonConverter<T> : JsonConverter<T> where T : Text, ICreate<T, string>
    {
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException();
            }

            string value = reader.GetString()!;
            var result = T.Create(value);

            if (result.IsFailure)
            {
                throw new JsonException($"Invalid value for {typeToConvert.Name}: {value}");
            }

            return result.Value;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}




