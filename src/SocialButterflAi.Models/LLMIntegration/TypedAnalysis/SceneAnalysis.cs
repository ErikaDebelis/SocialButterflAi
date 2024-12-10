using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.TypedAnalysis
{
    public class SceneAnalysis
    {
        [JsonProperty("Id")]
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }

        [JsonProperty("Certainty")]
        [JsonPropertyName("Certainty")]
        public double Certainty { get; set; }

        // [JsonProperty("Text")]
        // [JsonPropertyName("Text")]
        // public string Text { get; set; }

        // [JsonProperty("Sentiment")]
        // [JsonPropertyName("Sentiment")]
        // public string Sentiment { get; set; }

        // [JsonProperty("Tone")]
        // [JsonPropertyName("Tone")]
        // public IEnumerable<string> Tone { get; set; }

        public string EnhancedDescription { get; set; }
        public string EmotionalContext { get; set; }
        public string? NonVerbalCues { get; set; }
        public Dictionary<string, string>? Metadata { get; set; }
    }
}