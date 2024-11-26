using Newtonsoft.Json;
using System.Text.Json;

namespace ButterflAi.Models.Analysis
{
    public class UploadDtoRequest
    {
        [JsonProperty("TransactionId")]
        [JsonPropertyName("TransactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("File")]
        [JsonPropertyName("File")]
        public IFormFile File { get; set; }
    }
}