using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration
{
    public interface IBaseAiRequestRequirements
    {

        [JsonProperty("temperature")]
        [JsonPropertyName("temperature")]
        public float Temperature { get; set; }

        [JsonProperty("max_tokens")]
        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; }
    }
}