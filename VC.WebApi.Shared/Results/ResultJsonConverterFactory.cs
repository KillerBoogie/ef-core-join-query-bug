using System.Text.Json;
using System.Text.Json.Serialization;
using VC.WebApi.Shared.Json;

namespace VC.WebApi.Shared.Results
{
    public class ResultJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert == typeof(Result))
            {
                return true;
            }

            if (typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Result<>))
            {
                return true;
            }

            return false;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert == typeof(Result))
            {
                return new ResultJsonConverter();
            }

            if (typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Result<>))
            {
                Type itemType = typeToConvert.GetGenericArguments()[0];
                Type converterType = typeof(ResultJsonConverter<>).MakeGenericType(itemType);
                return (JsonConverter?)Activator.CreateInstance(converterType);
            }

            throw new InvalidOperationException("Cannot create converter for type: " + typeToConvert);
        }
    }

    public class ResultJsonConverter : JsonConverter<Result>
    {
        public override Result? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotSupportedException("Deserialization is not supported.");
        }

        public override void Write(Utf8JsonWriter writer, Result value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("state", value.IsSuccess ? "Success" : "Failure");

            if (value.IsFailure)
            {
                string propertyName = JsonUtility.ConvertNameAccordingToJsonNamingPolicy(options, "errors");
                writer.WritePropertyName(propertyName);
                JsonSerializer.Serialize(writer, value.Errors, options);
            }

            writer.WriteEndObject();
        }
    }

    public class ResultJsonConverter<T> : JsonConverter<Result<T>>
    {
        public override Result<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotSupportedException("Deserialization is not supported.");
        }

        public override void Write(Utf8JsonWriter writer, Result<T> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            string propertyName = JsonUtility.ConvertNameAccordingToJsonNamingPolicy(options, "state");
            writer.WriteString(propertyName, value.IsSuccess ? "Success" : "Failure");

            if (value.IsFailure)
            {
                propertyName = JsonUtility.ConvertNameAccordingToJsonNamingPolicy(options, "errors");
                writer.WritePropertyName(propertyName);
                JsonSerializer.Serialize(writer, value.Errors, options);
            }

            if (value.IsSuccess)
            {
                propertyName = JsonUtility.ConvertNameAccordingToJsonNamingPolicy(options, "value");
                writer.WritePropertyName(propertyName);
                JsonSerializer.Serialize(writer, value.Value, options);
            }

            writer.WriteEndObject();
        }
    }
}
