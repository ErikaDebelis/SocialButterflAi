using Newtonsoft.Json;
using System.Text.Json;

using ButterflAi.Models.Claude;

namespace ButterflAi.Models.Analysis
{
    public class AnalysisResponse
    {
        [JsonProperty("Success")]
        [JsonPropertyName("Success")]
        public bool Success { get; set; }

        [JsonProperty("Message")]
        [JsonPropertyName("Message")]
        public string Message { get; set; }

        [JsonProperty("Transcript")]
        [JsonPropertyName("Transcript")]
        public string Transcript { get; set; }

        [JsonProperty("Conclusion")]
        [JsonPropertyName("Conclusion")]
        public Conclusion Conclusion { get; set; }
    }
}