using Newtonsoft.Json;
using System.Text.Json;

namespace ButterflAi.Models.Claude
{
    public class ClaudeRequest
    {
        [JsonProperty("model")]
        [JsonPropertyName("model")]
        public Model Model => Model.Claude3mv5Sonnet20240620;

        [JsonProperty("system")]
        [JsonPropertyName("system")]
        public string SystemPrompt { get; set; }

        [JsonProperty("temperature")]
        [JsonPropertyName("temperature")]
        public float Temperature { get; set; }

        [JsonProperty("max_tokens")]
        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; }

        [JsonProperty("messages")]
        [JsonPropertyName("messages")]
        public IEnumerable<Message> Messages { get; set; }
    }
}