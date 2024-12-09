using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.OpenAi
{
    public class OpenAiRequest : BaseAiRequestRequirements
    {
        [JsonProperty("temperature")]
        [JsonPropertyName("temperature")]
        public override float Temperature => 0.0f;

        [JsonProperty("max_tokens")]
        [JsonPropertyName("max_tokens")]
        public override int MaxTokens => 1400;
    }
}