using Newtonsoft.Json;
using System.Text.Json;

namespace ButterflAi.Models.Integration
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