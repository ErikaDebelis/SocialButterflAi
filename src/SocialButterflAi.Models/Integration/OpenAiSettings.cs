using Newtonsoft.Json;
using System.Text.Json;

using ButterflAi.Models.OpenAi.Whisper;

namespace ButterflAi.Models.Integration
{
    public class OpenAiSettings
    {
        [JsonProperty("model")]
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonProperty("api-key")]
        [JsonPropertyName("api-key")]
        public string ApiKey { get; set; }

        [JsonProperty("Url")]
        [JsonPropertyName("Url")]
        public string Url { get; set; }
    }
}