using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using SocialButterflAi.Models.LLMIntegration.TypedAnalysis;

namespace SocialButterflAi.Models.Analysis
{
    public class UploadAndAnalysisData<T> where T : AnalysisData
    {
        [JsonProperty("AnalysisData")]
        [JsonPropertyName("AnalysisData")]
        public T AnalysisData { get; set; }

        [JsonProperty("Path")]
        [JsonPropertyName("Path")]
        public string Path { get; set; }
    }
}