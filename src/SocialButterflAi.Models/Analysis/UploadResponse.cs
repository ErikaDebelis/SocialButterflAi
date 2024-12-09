using Newtonsoft.Json;
using System.Text.Json.Serialization;

using SocialButterflAi.Models.LLMIntegration.Claude;

namespace SocialButterflAi.Models.Analysis
{
    public class UploadResponse : BaseResponse
    {
        [JsonProperty("VideoPath")]
        [JsonPropertyName("VideoPath")]
        public string VideoPath { get; set; }
    }
}