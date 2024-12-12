using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.Integration
{
    public class Settings
    {
        [JsonProperty("AnalysisSettings")]
        [JsonPropertyName("AnalysisSettings")]
        public AnalysisSettings AnalysisSettings { get; set; }

        [JsonProperty("OpenAi")]
        [JsonPropertyName("OpenAi")]
        public OpenAiSettings OpenAi { get; set; }

        [JsonProperty("Claude")]
        [JsonPropertyName("Claude")]
        public ClaudeSettings Claude { get; set; }
        
        [JsonProperty("Postgres")]
        [JsonPropertyName("Postgres")]
        public PostgresSettings Postgres { get; set; }
    }
}