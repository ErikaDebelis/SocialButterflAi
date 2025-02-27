using System;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models
{
    public class BaseDto
    {
        [JsonProperty("Id")]
        [JsonPropertyName("Id")]
        public virtual Guid? Id { get; set; }
    }
}