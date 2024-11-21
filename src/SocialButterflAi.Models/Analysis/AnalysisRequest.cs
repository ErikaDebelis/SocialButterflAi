using Newtonsoft.Json;
using System.Text.Json;

namespace ButterflAi.Models.Analysis
{
    public class AnalysisRequest
    {
        [JsonProperty("base64Audio")]
        [JsonPropertyName("base64Audio")]
        public string Base64Audio { get; set; }

        [JsonProperty("AudioFormat")]
        [JsonPropertyName("AudioFormat")]
        public AudioFormat AudioFormat { get; set; }

        [JsonProperty("Language")]
        [JsonPropertyName("Language")]
        public SupportedLanguages Language { get; set; }
    }
}