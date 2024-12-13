using Newtonsoft.Json;
using System.Text.Json.Serialization;

using SocialButterflAi.Models.LLMIntegration.OpenAi.Whisper;

namespace SocialButterflAi.Models.IntegrationSettings
{
    public class OpenAiSettings
    {
        [JsonProperty("WhisperModel")]
        [JsonPropertyName("WhisperModel")]
        public string WhisperModel { get; set; }

        [JsonProperty("CompletionModel")]
        [JsonPropertyName("CompletionModel")]
        public string CompletionModel { get; set; }

        [JsonProperty("ApiKey")]
        [JsonPropertyName("ApiKey")]
        public string ApiKey { get; set; }

        [JsonProperty("Url")]
        [JsonPropertyName("Url")]
        public string Url { get; set; }

        [JsonProperty("WhisperEndpoint")]
        [JsonPropertyName("WhisperEndpoint")]
        public string WhisperEndpoint { get; set; }

        [JsonProperty("CompletionEndpoint")]
        [JsonPropertyName("CompletionEndpoint")]
        public string CompletionEndpoint { get; set; }
    }
}