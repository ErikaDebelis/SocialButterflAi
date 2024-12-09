using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration
{
    public class BaseAiRequestRequirements: IBaseAiRequestRequirements
    {
        [JsonProperty("temperature")]
        [JsonPropertyName("temperature")]
        public virtual float Temperature { get; set; }

        [JsonProperty("max_tokens")]
        [JsonPropertyName("max_tokens")]
        public virtual int MaxTokens { get; set; }
    }
}