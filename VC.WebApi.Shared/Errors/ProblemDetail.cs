using System.Text.Json.Serialization;

namespace VC.WebApi.Shared.Errors
{
    public sealed class ProblemDetail(string type, string title, int status, string traceId)
    {
        [JsonPropertyOrder(-5)]
        [JsonPropertyName("type")]
        public string Type { get; init; } = type;

        [JsonPropertyOrder(-4)]
        [JsonPropertyName("title")]
        public string Title { get; init; } = title;

        [JsonPropertyOrder(-3)]
        [JsonPropertyName("status")]
        public int Status { get; init; } = status;

        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //[JsonPropertyOrder(-2)]
        //[JsonPropertyName("detail")]
        //public string? Detail { get; init; } = detail;

        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //[JsonPropertyOrder(-6)]
        //[JsonPropertyName("instance")]
        //public string? Instance { get; init; }

        [JsonExtensionData]
        public IDictionary<string, object?> Extensions { get; init; } = new Dictionary<string, object?>(StringComparer.Ordinal);

        [JsonPropertyName("traceId")]
        public string TraceId { get; init; } = traceId;

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; init; } = DateTime.UtcNow;

        public ProblemDetail(string type, string title, int status, string traceId, IDictionary<string, object?> extensions) : this(type, title, status, traceId)
        {
            Extensions = extensions; ;
        }
    }

    public class ErrorsDetail(string? type, string? pointer, string detail, IDictionary<string, object?> detailParameters, IDictionary<string, object?> extensions)
    {
        [JsonPropertyOrder(-4)]
        [JsonPropertyName("type")]
        public string? Type { get; init; } = type;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(-3)]
        [JsonPropertyName("pointer")]
        public string? Pointer { get; init; } = pointer;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(-2)]
        [JsonPropertyName("detail")]
        public string Detail { get; init; } = detail;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("detailParameters")]
        public IDictionary<string, object?>? DetailParameters { get; init; } = detailParameters.Count == 0 ? null : detailParameters;

        [JsonExtensionData]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary<string, object?>? ErrorsDetailExtensions { get; init; } = extensions.Count == 0 ? null : extensions;
    }
}
