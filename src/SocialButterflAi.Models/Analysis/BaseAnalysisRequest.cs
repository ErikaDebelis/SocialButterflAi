using System;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using SocialButterflAi.Models.LLMIntegration.OpenAi.Whisper;

namespace SocialButterflAi.Models.Analysis
{
    public class BaseAnalysisRequest
    {
        public Guid RequesterIdentityId { get; set; }

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