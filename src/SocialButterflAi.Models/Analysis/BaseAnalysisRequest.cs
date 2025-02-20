using System;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using SocialButterflAi.Models.LLMIntegration.OpenAi.Whisper;

namespace SocialButterflAi.Models.Analysis
{
    public abstract class BaseAnalysisRequest
    {
        public Guid RequesterIdentityId { get; set; }
        public Guid MessageId { get; set; }
        public Guid AnalysisMediumId { get; set; } //text, image, audio, video

        [JsonProperty("ModelProvider")]
        [JsonPropertyName("ModelProvider")]
        public string ModelProvider { get; set; }

        [JsonProperty("TransactionId")]
        [JsonPropertyName("TransactionId")]
        public string TransactionId { get; set; }

        /// <summary>
        /// 'i think they were being sarcastic'
        /// </summary>
        [JsonProperty("InitialUserPerception")]
        [JsonPropertyName("InitialUserPerception")]
        public string? InitialUserPerception { get; set; }
    }
}