using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.Analysis
{
    public class Tone
    {
        [JsonProperty("Id")]
        [JsonPropertyName("Id")]
        public Guid? Id { get; set; }

        [JsonProperty("primaryEmotion")]
        [JsonPropertyName("primaryEmotion")]
        public string PrimaryEmotion { get; set; } // "hurt"

        [JsonProperty("emotionalSpectrum")]
        [JsonPropertyName("emotionalSpectrum")]
        public Dictionary<string, double> EmotionalSpectrum { get; set; } // { "anger": 0.5, "joy": 0.3, "sadness": 0.2 }

        [JsonProperty("emotionalContext")]
        [JsonPropertyName("emotionalContext")]
        public string EmotionalContext { get; set; } // "their voice quivered"

        [JsonProperty("nonVerbalCues")]
        [JsonPropertyName("nonVerbalCues")]
        public string NonVerbalCues { get; set; } // "they were frowning"

        [JsonProperty("intensityScore")]
        [JsonPropertyName("intensityScore")]
        public double IntensityScore { get; set; } // 0.8
    }
}