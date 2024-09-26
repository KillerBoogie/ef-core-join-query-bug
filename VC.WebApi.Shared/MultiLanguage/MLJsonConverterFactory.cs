using System.Text.Json;
using System.Text.Json.Serialization;
using VC.WebApi.Shared.Json;
using VC.WebApi.Shared.Languages;


namespace VC.WebApi.Shared.MultiLanguage
{
    public class MLJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert.IsGenericType)
            {
                var genericType = typeToConvert.GetGenericTypeDefinition();
                return genericType == typeof(ML<>) || genericType == typeof(MLRequired<>);
            }
            return false;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var elementType = typeToConvert.GetGenericArguments()[0];
            var genericTypeDefinition = typeToConvert.GetGenericTypeDefinition();

            if (genericTypeDefinition == typeof(ML<>))
            {
                var converterType = typeof(MLJsonConverter<>).MakeGenericType(elementType);
                return Activator.CreateInstance(converterType) as JsonConverter;
            }
            else if (genericTypeDefinition == typeof(MLRequired<>))
            {
                var converterType = typeof(MLRequiredJsonConverter<>).MakeGenericType(elementType);
                return Activator.CreateInstance(converterType) as JsonConverter;
            }

            throw new InvalidOperationException($"Unsupported type in ML CreateConverter {genericTypeDefinition}.");
        }

        private class MLJsonConverter<T> : JsonConverter<ML<T>>
        {
            public override ML<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.StartArray)
                {
                    throw new JsonException();
                }

                ML<T> ml = new();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray)
                    {
                        return ml;
                    }

                    if (reader.TokenType != JsonTokenType.StartObject)
                    {
                        throw new JsonException();
                    }

                    reader.Read();
                    if (reader.TokenType != JsonTokenType.PropertyName)
                    {
                        throw new JsonException();
                    }

                    string propertyName = reader.GetString() ?? throw new JsonException();
                    if (propertyName != "language") throw new JsonException();

                    reader.Read();
                    string language = reader.GetString() ?? throw new JsonException();

                    reader.Read();
                    propertyName = reader.GetString() ?? throw new JsonException();
                    if (propertyName != "value") throw new JsonException();

                    reader.Read();
                    T? type = JsonSerializer.Deserialize<T>(ref reader, options) ?? throw new JsonException("Could not deserialize T from ML<T>.");

                    reader.Read();
                    if (reader.TokenType != JsonTokenType.EndObject)
                    {
                        throw new JsonException();
                    }

                    ml.Add(Language.Create(language).Value, type);
                }

                throw new JsonException();
            }

            public override void Write(Utf8JsonWriter writer, ML<T> value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();

                foreach (KeyValuePair<Language, T> mlItem in value.Values)
                {
                    writer.WriteStartObject();

                    string propertyName = JsonUtility.ConvertNameAccordingToJsonNamingPolicy(options, "language");
                    writer.WritePropertyName(propertyName);
                    writer.WriteStringValue(mlItem.Key.ToString());

                    propertyName = JsonUtility.ConvertNameAccordingToJsonNamingPolicy(options, "value");
                    writer.WritePropertyName(propertyName);

                    JsonSerializer.Serialize(writer, mlItem.Value, options);

                    writer.WriteEndObject();
                }

                writer.WriteEndArray();
            }
        }

        private class MLRequiredJsonConverter<T> : JsonConverter<MLRequired<T>>
        {
            public override MLRequired<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.StartArray)
                {
                    throw new JsonException();
                }

                MLRequired<T>? ml = default;


                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray)
                    {
                        if (ml is null)
                        {
                            throw new JsonException("An empty list can't be deserialized to MLRequiered<T>");
                        }
                        else
                        {
                            return ml;
                        }
                    }

                    if (reader.TokenType != JsonTokenType.StartObject)
                    {
                        throw new JsonException();
                    }

                    reader.Read();
                    if (reader.TokenType != JsonTokenType.PropertyName)
                    {
                        throw new JsonException();
                    }

                    string propertyName = reader.GetString() ?? throw new JsonException();
                    if (propertyName != "language") throw new JsonException();

                    reader.Read();
                    string language = reader.GetString() ?? throw new JsonException();

                    reader.Read();
                    propertyName = reader.GetString() ?? throw new JsonException();
                    if (propertyName != "value") throw new JsonException();

                    reader.Read();
                    T? type = JsonSerializer.Deserialize<T>(ref reader, options) ?? throw new JsonException("Could not deserialize T from MLRequired<T>.");

                    reader.Read();
                    if (reader.TokenType != JsonTokenType.EndObject)
                    {
                        throw new JsonException();
                    }

                    Language lang = Language.Create(language).Value;

                    if (ml is null)
                    {
                        ml = new MLRequired<T>(lang, type);
                    }
                    else
                    {
                        ml.Add(lang, type);
                    }
                }
                throw new JsonException();
            }


            public override void Write(Utf8JsonWriter writer, MLRequired<T> value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();

                foreach (KeyValuePair<Language, T> mlItem in value.Values)
                {
                    writer.WriteStartObject();

                    string propertyName = JsonUtility.ConvertNameAccordingToJsonNamingPolicy(options, "language");
                    writer.WritePropertyName(propertyName);
                    writer.WriteStringValue(mlItem.Key.ToString());

                    propertyName = JsonUtility.ConvertNameAccordingToJsonNamingPolicy(options, "value");
                    writer.WritePropertyName(propertyName);

                    JsonSerializer.Serialize(writer, mlItem.Value, options);

                    writer.WriteEndObject();
                }

                writer.WriteEndArray();

            }
        }
    }
}

