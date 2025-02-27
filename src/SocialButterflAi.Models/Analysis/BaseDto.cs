using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.Analysis
{
    public class BaseDto
    {
        [JsonProperty("Id")]
        [JsonPropertyName("Id")]
        public virtual Guid? Id { get; set; }
    }
}