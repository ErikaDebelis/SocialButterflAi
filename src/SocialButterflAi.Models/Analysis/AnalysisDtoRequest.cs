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
    }
}