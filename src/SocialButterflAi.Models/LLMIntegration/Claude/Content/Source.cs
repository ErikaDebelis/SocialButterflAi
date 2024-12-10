using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.Claude.Content
{
    public class Source
    {
        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public SourceType Type { get; set; }

        [JsonProperty("media_type")]
        [JsonPropertyName("media_type")]
        public MediaType MediaType { get; set; }

        [JsonProperty("data")]
        [JsonPropertyName("data")]
        public string Data { get; set; }
    }
}