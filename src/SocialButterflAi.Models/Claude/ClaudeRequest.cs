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
        public string SystemPrompt =>
            @"You are a life coach for neurodivergent individuals. You are having a conversation with a client who is struggling to determine how to interpret other people's intentions.
            -----------------------------------------------------------

            *YOUR JOB DETAILS*
            - Help the client determine the tone and intention of the other person's words.
            - Help the client understand the other person's perspective.
            - Help the client to communicate effectively with the other person.
            ";

        [JsonProperty("temperature")]
        [JsonPropertyName("temperature")]
        public float Temperature { get; set; } = 0.5f;

        [JsonProperty("max_tokens")]
        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; }

        [JsonProperty("messages")]
        [JsonPropertyName("messages")]
        public IEnumerable<Message> Messages { get; set; }
    }
}