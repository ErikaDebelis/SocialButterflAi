using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models
{
    public class BaseResponse<T>
    {
        [JsonProperty("Success")]
        [JsonPropertyName("Success")]
        public bool Success { get; set; }

        [JsonProperty("Message")]
        [JsonPropertyName("Message")]
        public string Message { get; set; }

        [JsonProperty("Data")]
        [JsonPropertyName("Data")]
        public T Data { get; set; }
    }
}