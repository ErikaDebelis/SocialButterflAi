using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using SocialButterflAi.Models.LLMIntegration.TypedAnalysis;

namespace SocialButterflAi.Models.Analysis
{
    public class UploadAndAnalysisData: AnalysisData
    {
        [JsonProperty("Path")]
        [JsonPropertyName("Path")]
        public string Path { get; set; }
    }
}