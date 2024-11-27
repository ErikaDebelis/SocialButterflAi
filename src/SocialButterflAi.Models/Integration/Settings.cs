using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.Integration
{
    public class Settings
    {
        [JsonProperty("openai")]
        [JsonPropertyName("openai")]
        public OpenAiSettings OpenAi { get; set; }

        [JsonProperty("claude")]
        [JsonPropertyName("claude")]
        public ClaudeSettings Claude { get; set; }
    }
}