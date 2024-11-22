using Newtonsoft.Json;
using System.Text.Json;

namespace ButterflAi.Models.Claude
{
    public class Conclusion
    {
        [JsonProperty("Certainty")]
        [JsonPropertyName("Certainty")]
        public double Certainty { get; set; }

        [JsonProperty("Text")]
        [JsonPropertyName("Text")]
        public string Text { get; set; }

        [JsonProperty("Sentiment")]
        [JsonPropertyName("Sentiment")]
        public string Sentiment { get; set; }

        [JsonProperty("Tone")]
        [JsonPropertyName("Tone")]
        public IEnumerable<string> Tone { get; set; }
    }
}