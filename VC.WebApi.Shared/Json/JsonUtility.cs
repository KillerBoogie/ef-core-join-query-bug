using System.Text.Json;

namespace VC.WebApi.Shared.Json
{
    public static class JsonUtility
    {
        public static string ConvertNameAccordingToJsonNamingPolicy(JsonSerializerOptions options, string propertyName)
        {
            if (options.PropertyNamingPolicy != null)
            {
                propertyName = options.PropertyNamingPolicy.ConvertName(propertyName);
            }

            return propertyName;
        }
    }
}
