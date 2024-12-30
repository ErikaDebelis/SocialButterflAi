using Newtonsoft.Json;
using System.Text.Json.Serialization;

using SocialButterflAi.Models.LLMIntegration.Claude;

namespace SocialButterflAi.Models.Analysis
{
    public class ProcessVideoData
    {
        [JsonProperty("Base64Audio")]
        [JsonPropertyName("Base64Audio")]
        public string Base64Audio { get; set; }

        [JsonProperty("Base64Media")]
        [JsonPropertyName("Base64Media")]
        public string Base64Media { get; set; }
    }
}