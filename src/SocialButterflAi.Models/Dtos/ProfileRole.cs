using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.Dtos
{
    public enum ProfileRole
    {
        [JsonProperty("Owner")]
        [JsonPropertyName("Owner")]
        Owner,
        [JsonProperty("Admin")]
        [JsonPropertyName("Admin")]
        Admin,
        [JsonProperty("Custom")]
        [JsonPropertyName("Custom")]
        Custom
    }
}