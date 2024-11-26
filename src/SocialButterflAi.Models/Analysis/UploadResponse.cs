using Newtonsoft.Json;
using System.Text.Json;

using ButterflAi.Models.Claude;

namespace ButterflAi.Models.Analysis
{
    public class UploadResponse : BaseResponse
    {
        [JsonProperty("VideoPath")]
        [JsonPropertyName("VideoPath")]
        public string VideoPath { get; set; }
    }
}