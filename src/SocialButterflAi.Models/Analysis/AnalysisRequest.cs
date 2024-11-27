using Newtonsoft.Json;
using System.Text.Json.Serialization;
using SocialButterflAi.Models.OpenAi.Whisper;

namespace SocialButterflAi.Models.Analysis
{
    public class AnalysisRequest
    {
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

        [JsonProperty("Language")]
        [JsonPropertyName("Language")]
        public SupportedLanguages Language { get; set; }

        /// <summary>
        /// 'i think they were being sarcastic'
        /// </summary>
        [JsonProperty("Perception")]
        [JsonPropertyName("Perception")]
        public string Perception { get; set; }
    }
}