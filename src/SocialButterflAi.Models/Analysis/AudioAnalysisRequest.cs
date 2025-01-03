using System;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using SocialButterflAi.Models.LLMIntegration.OpenAi.Whisper;

namespace SocialButterflAi.Models.Analysis
{
    public class AudioAnalysisRequest : BaseAnalysisRequest
    {
        [JsonProperty("AudioId")]
        [JsonPropertyName("AudioId")]
        public Guid? AudioId { get; set; }

        [JsonProperty("MessageId")]
        [JsonPropertyName("MessageId")]
        public Guid? MessageId { get; set; }

        [JsonProperty("Base64Audio")]
        [JsonPropertyName("Base64Audio")]
        public string Base64Audio { get; set; }
    }
}