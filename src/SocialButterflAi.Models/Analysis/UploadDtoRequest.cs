using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.Analysis
{
    public class UploadDtoRequest
    {
        [JsonProperty("TransactionId")]
        [JsonPropertyName("TransactionId")]
        public string TransactionId { get; set; }
        
        [JsonProperty("ChatId")]
        [JsonPropertyName("ChatId")]
        public string? ChatId { get; set; }
        
        [JsonProperty("Title")]
        [JsonPropertyName("Title")]
        public string? Title { get; set; }
        
        [JsonProperty("Description")]
        [JsonPropertyName("Description")]
        public string? Description { get; set; }

    }
}