using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using SocialButterflAi.Models.LLMIntegration.TypedAnalysis;

namespace SocialButterflAi.Models.Analysis
{
    public class AnalysisResponse : BaseResponse
    {
        [JsonProperty("Transcript")]
        [JsonPropertyName("Transcript")]
        public string? Transcript { get; set; }
        //
        [JsonProperty("")]
        [JsonPropertyName("")]
        public IEnumerable<EnhancedCaption>? EnhancedCaptions { get; set; }

        [JsonProperty("Conclusion")]
        [JsonPropertyName("Conclusion")]
        public string? Conclusion { get; set; }
    }
}