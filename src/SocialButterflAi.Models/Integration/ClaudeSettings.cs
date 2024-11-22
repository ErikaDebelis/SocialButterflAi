using Newtonsoft.Json;
using System.Text.Json;

namespace ButterflAi.Models.Integration
{
    public class ClaudeSettings
    {
        [JsonProperty("anthropic-version")]
        [JsonPropertyName("anthropic-version")]
        public string Version { get; set; }

        [JsonProperty("model")]
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonProperty("x-api-key")]
        [JsonPropertyName("x-api-key")]
        public string ApiKey { get; set; }

        [JsonProperty("Url")]
        [JsonPropertyName("Url")]
        public string Url { get; set; }
    }
}