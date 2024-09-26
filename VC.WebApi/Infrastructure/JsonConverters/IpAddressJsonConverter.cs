using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VC.WebApi.Infrastructure.JsonConverters
{
    public class IpAddressJsonConverter : JsonConverter<IPAddress>
    {
        public override IPAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetString() is null ? null : IPAddress.Parse(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, IPAddress ipAddressValue, JsonSerializerOptions options)
            => writer.WriteStringValue(ipAddressValue.ToString());
    }
}
