using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.OpenAi
{
    public class TextContent: IContent
    {
        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public Type Type => Type.text;

        [JsonProperty("text")]
        [JsonPropertyName("text")]
        public Text Text { get; set; }
    }
}