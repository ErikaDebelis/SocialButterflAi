using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SocialButterFlAi.Data.Identity.Entities
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