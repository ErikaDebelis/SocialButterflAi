using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration
{
    public class AiRequest<T> where T: BaseAiRequestRequirements
    {
        [JsonPropertyName("AiData")]
        [JsonProperty("AiData")]
        public T AiData { get; set; }
    }
}