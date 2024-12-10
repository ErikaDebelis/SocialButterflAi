using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration
{
    public class BaseAiResponse<T> where T : BaseAiResponseRequirements
    {
        [JsonPropertyName("Success")]
        [JsonProperty("Success")]
        public bool Success { get; set; }

        [JsonPropertyName("Message")]
        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonPropertyName("AiData")]
        [JsonProperty("AiData")]
        public T AiData { get; set; }

    }
}