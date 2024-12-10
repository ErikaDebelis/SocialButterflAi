using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.OpenAi
{
    public class OpenAiMessage : Message
    {
        [JsonProperty("role")]
        [JsonPropertyName("role")]
        public Role Role { get; set; }

        [JsonProperty("content")]
        [JsonPropertyName("content")]
        public IContent Content { get; set; }
    }
}