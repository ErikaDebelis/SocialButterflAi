using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using SocialButterflAi.Models.LLMIntegration.TypedAnalysis;

namespace SocialButterflAi.Models.Analysis
{
    public class AnalysisData
    {
        [JsonProperty("caption")]
        [JsonPropertyName("caption")]
        public EnhancedCaption Caption { get; set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public AnalysisType? Type { get; set; }

        [JsonProperty("certainty")]
        [JsonPropertyName("certainty")]
        public double Certainty { get; set; }

        [JsonProperty("enhancedDescription")]
        [JsonPropertyName("enhancedDescription")]
        public string EnhancedDescription { get; set; }

        [JsonProperty("emotionalContext")]
        [JsonPropertyName("emotionalContext")]
        public string EmotionalContext { get; set; }

        [JsonProperty("nonVerbalCues")]
        [JsonPropertyName("nonVerbalCues")]
        public string? NonVerbalCues { get; set; }

        [JsonProperty("metadata")]
        [JsonPropertyName("metadata")]
        public Dictionary<string, string>? Metadata { get; set; }
    }
}