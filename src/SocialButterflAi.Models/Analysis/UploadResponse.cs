using Newtonsoft.Json;
using System.Text.Json.Serialization;

using SocialButterflAi.Models.LLMIntegration.Claude;

namespace SocialButterflAi.Models.Analysis
{
    public class UploadData
    {
        [JsonProperty("Path")]
        [JsonPropertyName("Path")]
        public string Path { get; set; }
    }
}