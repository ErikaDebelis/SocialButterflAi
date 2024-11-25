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
            ------------------
            - Help the client determine the tone and intention of the other person's words. (ex. input: ""I'm reeaalllyy happy for you..."" you might say, ""It sounds like they're being sarcastic and may have some other feelings they're not expressing."")
            - Help the client understand the other person's perspective.(ex. input: ""I'm reeaalllyy happy for you...""  you might say, ""if they're being sarcastic, they might be feeling jealous or upset. Can you think of a reason they might feel that way?"")
            - Help the client to communicate effectively with the other person. (ex. input: ""I'm reeaalllyy happy for you..."" you might say, ""You might want to ask them if they're feeling okay. It sounds like they might be upset about something."")
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