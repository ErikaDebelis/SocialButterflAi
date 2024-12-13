using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SocialButterflAi.Models.LLMIntegration.OpenAi.Content;

namespace SocialButterflAi.Models.LLMIntegration.OpenAi
{
    public class OpenAiMessage : Message
    {
        [JsonProperty("content")]
        [JsonPropertyName("content")]
        public new IEnumerable<IContent>? Content { get; set; }
    }
}