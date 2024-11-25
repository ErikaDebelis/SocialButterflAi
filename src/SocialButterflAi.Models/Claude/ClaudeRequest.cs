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
        public string SystemPrompt => "System"; //todo: make this meaningful


        [JsonProperty("temperature")]
        [JsonPropertyName("temperature")]
        public float Temperature => 0.5f;

        [JsonProperty("max_tokens")]
        [JsonPropertyName("max_tokens")]
        public int MaxTokens => 1000;

        [JsonProperty("messages")]
        [JsonPropertyName("messages")]
        public IEnumerable<Message> Messages => new List<Message>()
        {
            UserAnalysisPrimer.Message,
            AssistantAnalysisPrimer.Message
        }
    }
}