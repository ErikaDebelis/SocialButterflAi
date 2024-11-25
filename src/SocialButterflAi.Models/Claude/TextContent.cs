using Newtonsoft.Json;
using System.Text.Json;

namespace ButterflAi.Models.Claude
{
    public enum ContentType
    {
        Text
    }

    public class TextContent
    {
        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public ContentType Type { get; set; }

        [JsonProperty("text")]
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}