using Newtonsoft.Json;
using System.Text.Json;

namespace ButterflAi.Models.Claude
{
    public class Message
    {
        [JsonProperty("role")]
        [JsonPropertyName("role")]
        public Role Role { get; set; }

        [JsonProperty("content")]
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}