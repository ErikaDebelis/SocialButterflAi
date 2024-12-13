using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.IntegrationSettings
{
    public class PostgresSettings
    {
        [JsonProperty("Host")]
        [JsonPropertyName("Host")]
        public string Host { get; set; }

        [JsonProperty("Port")]
        [JsonPropertyName("Port")]
        public string Port { get; set; }

        [JsonProperty("Database")]
        [JsonPropertyName("Database")]
        public string Database { get; set; }

        [JsonProperty("Username")]
        [JsonPropertyName("Username")]
        public string Username { get; set; }

        [JsonProperty("Password")]
        [JsonPropertyName("Password")]
        public string Password { get; set; }

        [JsonProperty("IncludeErrorDetail")]
        [JsonPropertyName("IncludeErrorDetail")]
        public string IncludeErrorDetail { get; set; }
    }
}