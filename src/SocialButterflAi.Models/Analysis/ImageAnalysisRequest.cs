using System;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using SocialButterflAi.Models.LLMIntegration.OpenAi.Whisper;

namespace SocialButterflAi.Models.Analysis
{
    public class ImageAnalysisRequest: BaseAnalysisRequest
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("Transcript")]
        [JsonPropertyName("Transcript")]
        public string? Transcript { get; set; }
    }
}