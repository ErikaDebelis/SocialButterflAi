using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.OpenAi.Content
{
    public class ImageUrlContent: IContent
    {
        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public ContentType Type => ContentType.image_url;

        [JsonProperty("image_url")]
        [JsonPropertyName("image_url")]
        public ImageUrl ImageUrl { get; set; }
    }
}