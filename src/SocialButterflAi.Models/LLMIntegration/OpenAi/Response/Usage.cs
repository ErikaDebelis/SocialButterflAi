using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.OpenAi.Response
{
    ///
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
    ///
    public class Usage
    {
        [JsonProperty("prompt_tokens")]
        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens { get; set; }

        [JsonProperty("completion_tokens")]
        [JsonPropertyName("completion_tokens")]
        public int CompletionTokens { get; set; }

        [JsonProperty("total_tokens")]
        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }

        [JsonProperty("completion_tokens_details")]
        [JsonPropertyName("completion_tokens_details")]
        public CompletionTokensDetails CompletionTokensDetails { get; set; }
    }
}