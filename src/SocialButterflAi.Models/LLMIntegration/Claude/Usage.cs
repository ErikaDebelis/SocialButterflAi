using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.Claude
{
    public class Usage
    {
        [JsonProperty("input_tokens")]
        [JsonPropertyName("input_tokens")]
        public int InputTokens { get; set; }

        [JsonProperty("output_tokens")]
        [JsonPropertyName("output_tokens")]
        public int OutputTokens { get; set; }
    }
}