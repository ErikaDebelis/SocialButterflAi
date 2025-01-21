using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.Analysis
{
    public class Intent
    {
        [JsonProperty("Id")]
        [JsonPropertyName("Id")]
        public Guid? Id { get; set; }

        [JsonProperty("primaryIntent")]
        [JsonPropertyName("primaryIntent")]
        public string PrimaryIntent { get; set; } // "convince them to buy a car"

        [JsonProperty("secondaryIntents")]
        [JsonPropertyName("secondaryIntents")]
        public Dictionary<string, double> SecondaryIntents { get; set; } // { "make them feel good": 0.5, "make them feel safe": 0.3 }

        [JsonProperty("certaintyScore")]
        [JsonPropertyName("certaintyScore")]
        public double CertaintyScore { get; set; } // 0.8

        [JsonProperty("subtextualMeaning")]
        [JsonPropertyName("subtextualMeaning")]
        public string SubtextualMeaning { get; set; } // "they are working hard to make a sale"
    }
}