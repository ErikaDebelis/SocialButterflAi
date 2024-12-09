using Newtonsoft.Json;
using System.Text.Json.Serialization;

using SocialButterflAi.Models.LLMIntegration.OpenAi.Whisper;

namespace SocialButterflAi.Models.Integration
{
    public class OpenAiSettings
    {
        [JsonProperty("Model")]
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