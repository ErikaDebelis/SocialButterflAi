using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.Dtos
{
    public enum Title
    {
        [JsonProperty("Dr")]
        [JsonPropertyName("Dr")]
        Dr,
        [JsonProperty("Miss")]
        [JsonPropertyName("Miss")]
        Miss,
        [JsonProperty("Mr")]
        [JsonPropertyName("Mr")]
        Mr,
        [JsonProperty("Mrs")]
        [JsonPropertyName("Mrs")]
        Mrs,
        [JsonProperty("Ms")]
        [JsonPropertyName("Ms")]
        Ms,
        [JsonProperty("Mx")]
        [JsonPropertyName("Mx")]
        Mx,
        [JsonProperty("Other")]
        [JsonPropertyName("Other")]
        Other
    }
}