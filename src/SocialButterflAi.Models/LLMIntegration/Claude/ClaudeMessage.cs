using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.Claude
{
    public class ClaudeMessage : Message
    {
        [JsonProperty("role")]
        [JsonPropertyName("role")]
        public Role Role { get; set; }
    }
}