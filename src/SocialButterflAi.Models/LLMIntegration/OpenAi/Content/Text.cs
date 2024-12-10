using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.OpenAi.Content
{
    public class Text
    {
        [JsonProperty("value")]
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonProperty("annotations")]
        [JsonPropertyName("annotations")]
        public IEnumerable<string>? Annotations { get; set; }
    }
}