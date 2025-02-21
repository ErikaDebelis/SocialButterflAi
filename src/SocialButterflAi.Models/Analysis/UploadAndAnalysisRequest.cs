using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using SocialButterflAi.Models.LLMIntegration.TypedAnalysis;

namespace SocialButterflAi.Models.Analysis
{
    public class UploadAndAnalysisRequest<T> where T : BaseAnalysisRequest
    {
        [JsonProperty("Data")]
        [JsonPropertyName("Data")]
        public T Data { get; set; }
    }
}