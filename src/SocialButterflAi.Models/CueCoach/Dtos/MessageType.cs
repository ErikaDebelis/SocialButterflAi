using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SocialButterflAi.Models.CueCoach.Dtos
{
    public enum MessageType
    {
        [JsonProperty("Text")]
        [JsonPropertyName("Text")]
        Text,
        [JsonProperty("Image")]
        [JsonPropertyName("Image")]
        Image,
        [JsonProperty("Video")]
        [JsonPropertyName("Video")]
        Video,
        [JsonProperty("Audio")]
        [JsonPropertyName("Audio")]
        Audio,
        [JsonProperty("System")]
        [JsonPropertyName("System")]
        System
    }
}