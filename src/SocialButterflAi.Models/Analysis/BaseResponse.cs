using Newtonsoft.Json;
using System.Text.Json;

using ButterflAi.Models.Claude;

namespace ButterflAi.Models.Analysis
{
    public class BaseResponse
    {
        [JsonProperty("Success")]
        [JsonPropertyName("Success")]
        public bool Success { get; set; }

        [JsonProperty("Message")]
        [JsonPropertyName("Message")]
        public string Message { get; set; }
    }
}