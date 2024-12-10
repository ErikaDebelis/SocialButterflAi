using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.OpenAi
{
    public class ImageFileContent: IContent
    {
        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public ContentType Type => ContentType.image_file;

        [JsonProperty("image_file")]
        [JsonPropertyName("image_file")]
        public ImageFile ImageFile { get; set; }
    }
}