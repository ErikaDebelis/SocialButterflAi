using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.OpenAi
{
    public class OpenAiRequest : BaseAiRequestRequirements
    {
        [JsonProperty("model")]
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonProperty("prompt")]
        [JsonPropertyName("prompt")]
        public string Prompt => new AssistantAnalysisPrimer().Message.Content;

        [JsonProperty("temperature")]
        [JsonPropertyName("temperature")]
        public override float Temperature => 0.0f;

        [JsonProperty("max_tokens")]
        [JsonPropertyName("max_tokens")]
        public override int MaxTokens => 1400;
    }
}