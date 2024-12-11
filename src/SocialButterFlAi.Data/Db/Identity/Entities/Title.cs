using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterFlAi.Data.Db.Identity.Entities
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
        [JsonProperty("Mx")] //salutation that does not indicate gender
        [JsonPropertyName("Mx")]
        Mx,
        [JsonProperty("Other")]
        [JsonPropertyName("Other")]
        Other
    }
}