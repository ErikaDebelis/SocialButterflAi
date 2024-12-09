using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.Claude
{
    public class Message
    {
        [JsonProperty("role")]
        [JsonPropertyName("role")]
        public Role Role { get; set; }

        [JsonProperty("content")]
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}