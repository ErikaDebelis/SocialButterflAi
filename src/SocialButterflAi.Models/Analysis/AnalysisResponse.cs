using Newtonsoft.Json;
using System.Text.Json;

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

        [JsonProperty("Text")]
        [JsonPropertyName("Text")]
        public string Text { get; set; }
    }
}