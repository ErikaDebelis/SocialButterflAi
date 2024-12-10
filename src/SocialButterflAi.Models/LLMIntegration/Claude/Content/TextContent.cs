using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.Claude.Content
{
    public class TextContent: IContent
    {
        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public ContentType Type => ContentType.Text;

        [JsonProperty("text")]
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}