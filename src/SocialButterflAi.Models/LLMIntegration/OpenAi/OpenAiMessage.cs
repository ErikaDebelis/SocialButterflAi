using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.OpenAi
{
    public class OpenAiMessage : Message
    {
        [JsonProperty("content")]
        [JsonPropertyName("content")]
        public IEnumerable<IContent>? Content { get; set; }
    }
}