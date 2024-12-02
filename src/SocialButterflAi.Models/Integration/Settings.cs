using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.Integration
{
    public class Settings
    {
        [JsonProperty("AnalysisSettings")]
        [JsonPropertyName("AnalysisSettings")]
        public AnalysisSettings AnalysisSettings { get; set; }

        [JsonProperty("Openai")]
        [JsonPropertyName("Openai")]
        public OpenAiSettings OpenAi { get; set; }

        [JsonProperty("Claude")]
        [JsonPropertyName("Claude")]
        public ClaudeSettings Claude { get; set; }
    }
}