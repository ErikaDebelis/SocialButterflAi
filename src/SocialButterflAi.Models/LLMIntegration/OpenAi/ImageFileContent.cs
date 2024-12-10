using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.OpenAi
{
    public class ImageFileContent: IContent
    {
        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public Type Type => Type.image_file;

        [JsonProperty("image_file")]
        [JsonPropertyName("image_file")]
        public ImageFile ImageFile { get; set; }
    }
}