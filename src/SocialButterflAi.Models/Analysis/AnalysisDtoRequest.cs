using System;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using SocialButterflAi.Models.LLMIntegration.OpenAi.Whisper;

namespace SocialButterflAi.Models.Analysis
{
    public class AnalysisDtoRequest
    {
        public Guid RequesterIdentityId { get; set; }
        
        [JsonProperty("ModelProvider")]
        [JsonPropertyName("ModelProvider")]
        public string ModelProvider { get; set; }

        [JsonProperty("TransactionId")]
        [JsonPropertyName("TransactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("VideoPath")]
        [JsonPropertyName("VideoPath")]
        public string VideoPath { get; set; }

        /// <summary>
        /// the timeframe of the video to be analyzed - start time 00:00:00, end time 00:00:00 - default is the entire video
        /// </summary>
        [JsonProperty("StartTime")]
        [JsonPropertyName("StartTime")]
        public string StartTime { get; set; } = "00:00:00"; // Default start

        [JsonProperty("EndTime")]
        [JsonPropertyName("EndTime")]
        public string? EndTime { get; set; } = ""; // Empty means until the end


        /// <summary>
        /// 'i think they were being sarcastic'
        /// </summary>
        [JsonProperty("InitialUserPerception")]
        [JsonPropertyName("InitialUserPerception")]
        public string InitialUserPerception { get; set; }
    }
}