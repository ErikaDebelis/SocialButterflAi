using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using SocialButterflAi.Models.LLMIntegration.Claude;

namespace SocialButterflAi.Models.LLMIntegration.OpenAi.Response
{
    ///
    //     {
    //     "id": "chatcmpl-123",
    //     "object": "chat.completion",
    //     "created": 1677652288,
    //     "model": "gpt-4o-mini",
    //     "system_fingerprint": "fp_44709d6fcb",
    //     "choices": [{
    //         "index": 0,
    //         "message": {
    //             "role": "assistant",
    //             "content": "\n\nHello there, how may I assist you today?",
    //         },
    //         "logprobs": null,
    //         "finish_reason": "stop"
    //     }],
    //     "usage": {
    //         "prompt_tokens": 9,
    //         "completion_tokens": 12,
    //         "total_tokens": 21,
    //         "completion_tokens_details": {
    //             "reasoning_tokens": 0,
    //             "accepted_prediction_tokens": 0,
    //             "rejected_prediction_tokens": 0
    //         }
    //     }
    // }
    ///
    public class OpenAiResponse: BaseAiResponseRequirements
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public override string Id { get; set; }

        [JsonProperty("object")]
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonProperty("created")]
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonProperty("model")]
        [JsonPropertyName("model")]
        public Model Model { get; set; }

        [JsonProperty("system_fingerprint")]
        [JsonPropertyName("system_fingerprint")]
        public string SystemFingerprint { get; set; }

        [JsonProperty("choices")]
        [JsonPropertyName("choices")]
        public IEnumerable<Choice> Choices { get; set; }

        [JsonProperty("usage")]
        [JsonPropertyName("usage")]
        public Usage Usage { get; set; }
    }
}