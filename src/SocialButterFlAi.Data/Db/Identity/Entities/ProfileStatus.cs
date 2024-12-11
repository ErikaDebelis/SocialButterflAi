using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterFlAi.Data.Db.Identity.Entities
{
    public enum ProfileStatus
    {
        [JsonProperty("Active")]
        [JsonPropertyName("Active")]
        Active,
        [JsonProperty("Deactivated")]
        [JsonPropertyName("Deactivated")]
        Deactivated,
        [JsonProperty("Unknown")]
        [JsonPropertyName("Unknown")]
        Unknown
    }
}