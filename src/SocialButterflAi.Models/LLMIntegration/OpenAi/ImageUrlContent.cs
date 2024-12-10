using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.OpenAi
{
    public class ImageUrlContent: IContent
    {
        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public Type Type => Type.image_url;

        [JsonProperty("image_url")]
        [JsonPropertyName("image_url")]
        public ImageUrl ImageUrl { get; set; }
    }
}