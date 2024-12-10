using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.Claude.Content
{
    public class ImageContent: IContent
    {
        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public ContentType Type => ContentType.Image;

        [JsonProperty("source")]
        [JsonPropertyName("source")]
        public Source Source { get; set; }
    }
}