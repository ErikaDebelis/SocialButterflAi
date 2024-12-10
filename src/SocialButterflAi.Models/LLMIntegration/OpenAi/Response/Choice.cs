using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.OpenAi.Response
{
    ///
    // "choices": [
    //   {
    //     "index": 0,
    //     "message": {
    //       "role": "assistant",
    //       "content": "\n\nHello there, how may I assist you today?",
    //     },
    //     "logprobs": null,
    //     "finish_reason": "stop"
    //   }
    // ]
    ///
    public class Choice
    {
        [JsonProperty("index")]
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonProperty("message")]
        [JsonPropertyName("message")]
        public OpenAiMessage Message { get; set; }

        [JsonProperty("logprobs")]
        [JsonPropertyName("logprobs")]
        public int? LogProbabilities { get; set; }

        [JsonProperty("finish_reason")]
        [JsonPropertyName("finish_reason")]
        public FinishReason FinishReason { get; set; }
    }
}