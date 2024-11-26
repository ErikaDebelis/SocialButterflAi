using Newtonsoft.Json;
using System.Text.Json;

namespace ButterflAi.Models.Analysis
{
    public class AnalysisDtoRequest
    {

        [JsonProperty("TransactionId")]
        [JsonPropertyName("TransactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("GifPath")]
        [JsonPropertyName("GifPath")]
        public string GifPath { get; set; }

        [JsonProperty("AudioPath")]
        [JsonPropertyName("AudioPath")]
        public string AudioPath { get; set; }

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