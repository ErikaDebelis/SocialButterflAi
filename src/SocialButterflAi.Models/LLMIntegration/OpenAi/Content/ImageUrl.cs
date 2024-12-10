using Newtonsoft.Json;
using SocialButterflAi.Models.LLMIntegration.OpenAi;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.OpenAi.Content
{
    public class ImageUrl
    {
        [JsonProperty("url")]
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonProperty("detail")]
        [JsonPropertyName("detail")]
        public ImageDetail Detail { get; set; }
    }
}