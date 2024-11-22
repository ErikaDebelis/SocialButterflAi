using Newtonsoft.Json;
using System.Text.Json;

namespace ButterflAi.Models.Analysis
{
    public class AnalysisDtoRequest
    {
        [JsonProperty("AudioFile")]
        [JsonPropertyName("AudioFile")]
        public IFormFile AudioFile { get; set; }

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