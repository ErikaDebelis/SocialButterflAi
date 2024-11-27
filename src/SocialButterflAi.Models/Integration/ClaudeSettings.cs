using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.Integration
{
    public class ClaudeSettings
    {
        [JsonProperty("Version")]
        [JsonPropertyName("Version")]
        public string Version { get; set; }

        [JsonProperty("model")]
        [JsonPropertyName("Model")]
        public string Model { get; set; }

        [JsonProperty("ApiKey")]
        [JsonPropertyName("ApiKey")]
        public string ApiKey { get; set; }

        [JsonProperty("Url")]
        [JsonPropertyName("Url")]
        public string Url { get; set; }
    }
}