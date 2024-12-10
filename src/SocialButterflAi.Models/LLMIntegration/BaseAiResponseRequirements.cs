using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration
{
    public class BaseAiResponseRequirements: IBaseAiResponseRequirements
    {
        [JsonProperty("Id")]
        [JsonPropertyName("Id")]
        public virtual string Id { get; set; }
    }
}