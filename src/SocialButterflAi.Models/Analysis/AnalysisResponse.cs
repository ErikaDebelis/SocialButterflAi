using Newtonsoft.Json;
using System.Text.Json;

using ButterflAi.Models.Claude;

namespace ButterflAi.Models.Analysis
{
    public class AnalysisResponse : BaseResponse
    {
        [JsonProperty("Transcript")]
        [JsonPropertyName("Transcript")]
        public string? Transcript { get; set; }

        [JsonProperty("Conclusion")]
        [JsonPropertyName("Conclusion")]
        public Conclusion? Conclusion { get; set; }
    }
}