using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.CueCoach.Dtos
{
    public enum Gender
    {
        [JsonProperty("Male")]
        [JsonPropertyName("Male")]
        Male,
        [JsonProperty("Female")]
        [JsonPropertyName("Female")]
        Female,
        [JsonProperty("NonBinary")]
        [JsonPropertyName("NonBinary")]
        NonBinary,
        [JsonProperty("Other")]
        [JsonPropertyName("Other")]
        Other
    }
}