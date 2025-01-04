using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.CueCoach.Dtos
{
    public enum Pronoun
    {
        [JsonProperty("He")]
        [JsonPropertyName("He")]
        He,
        [JsonProperty("Him")]
        [JsonPropertyName("Him")]
        Him,
        [JsonProperty("Her")]
        [JsonPropertyName("Her")]
        Her,
        [JsonProperty("She")]
        [JsonPropertyName("She")]
        She,
        [JsonProperty("Them")]
        [JsonPropertyName("Them")]
        Them,
        [JsonProperty("They")]
        [JsonPropertyName("They")]
        They
    }
}