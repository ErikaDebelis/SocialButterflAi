using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.CueCoach.Dtos
{
    public enum ChatStatus
    {
        [JsonProperty("Unknown")]
        [JsonPropertyName("Unknown")]
        Unknown,
        [JsonProperty("Active")]
        [JsonPropertyName("Active")]
        Active,
        [JsonProperty("Archived")]
        [JsonPropertyName("Archived")]
        Archived,
        [JsonProperty("Disabled")]
        [JsonPropertyName("Disabled")]
        Disabled
    }
}