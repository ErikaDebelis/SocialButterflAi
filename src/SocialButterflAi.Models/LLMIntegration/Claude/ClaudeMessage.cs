using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using SocialButterflAi.Models.LLMIntegration.Claude.Content;

namespace SocialButterflAi.Models.LLMIntegration.Claude
{
    public class ClaudeMessage : Message
    {
        [JsonProperty("content")]
        [JsonPropertyName("content")]
        public new IEnumerable<IContent>? Content { get; set; }
    }
}