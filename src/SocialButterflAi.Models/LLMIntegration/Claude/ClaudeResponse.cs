using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using SocialButterflAi.Models.LLMIntegration.Claude.Content;

namespace SocialButterflAi.Models.LLMIntegration.Claude
{
    public enum ResponseType
    {
        Message
    }

    public class ClaudeResponse
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public ResponseType Type { get; set; } = ResponseType.Message;

        [JsonProperty("role")]
        [JsonPropertyName("role")]
        public Role Role { get; set; } = Role.Assistant;

        [JsonProperty("content")]
        [JsonPropertyName("content")]
        public IEnumerable<IContent> Content { get; set; }

        [JsonProperty("model")]
        [JsonPropertyName("model")]
        public Model Model { get; set; } = Model.Claude3mv5Sonnet20241022;

        [JsonProperty("stop_reason")]
        [JsonPropertyName("stop_reason")]
        public StopReason StopReason { get; set; }

        [JsonProperty("stop_sequence")]
        [JsonPropertyName("stop_sequence")]
        public string? StopSequence { get; set; }

        [JsonProperty("usage")]
        [JsonPropertyName("usage")]
        public Usage Usage { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}