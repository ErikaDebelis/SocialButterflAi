using Newtonsoft.Json;
using System.Text.Json.Serialization;

using SocialButterflAi.Models.Claude;

namespace SocialButterflAi.Models.Analysis
{
    public class AnalysisResponse : BaseResponse
    {
        [JsonProperty("Transcript")]
        [JsonPropertyName("Transcript")]
        public string? Transcript { get; set; }
        //
        // [JsonProperty("Conclusion")]
        // [JsonPropertyName("Conclusion")]
        // public Conclusion? Conclusion { get; set; }

        [JsonProperty("Conclusion")]
        [JsonPropertyName("Conclusion")]
        public string? Conclusion { get; set; }
    }
}