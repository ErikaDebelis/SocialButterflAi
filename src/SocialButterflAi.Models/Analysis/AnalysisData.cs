using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using SocialButterflAi.Models.LLMIntegration.TypedAnalysis;

namespace SocialButterflAi.Models.Analysis
{
    public class AnalysisData
    {
        [JsonProperty("identityId")]
        [JsonPropertyName("identityId")]
        public Guid? IdentityId { get; set; }

        [JsonProperty("Id")]
        [JsonPropertyName("Id")]
        public Guid? Id { get; set; }

        [JsonProperty("caption")]
        [JsonPropertyName("caption")]
        public EnhancedCaption? Caption { get; set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public MediaType? Type { get; set; }

        [JsonProperty("certainty")]
        [JsonPropertyName("certainty")]
        public double Certainty { get; set; }

        [JsonProperty("enhancedDescription")]
        [JsonPropertyName("enhancedDescription")]
        public string EnhancedDescription { get; set; }

        [JsonProperty("tone")]
        [JsonPropertyName("tone")]
        public Tone Tone { get; set; }

        [JsonProperty("intent")]
        [JsonPropertyName("intent")]
        public Intent Intent { get; set; }

        [JsonProperty("metadata")]
        [JsonPropertyName("metadata")]
        public Dictionary<string, string>? Metadata { get; set; }
    }
}