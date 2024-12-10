using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.OpenAi.Response
{
    ///
    //  "completion_tokens_details": {
    //      "reasoning_tokens": 0,
    //      "accepted_prediction_tokens": 0,
    //      "rejected_prediction_tokens": 0
    //  }
    ///
    public class CompletionTokensDetails
    {
        [JsonProperty("reasoning_tokens")]
        [JsonPropertyName("reasoning_tokens")]
        public int ReasoningTokens { get; set; }

        [JsonProperty("accepted_prediction_tokens")]
        [JsonPropertyName("accepted_prediction_tokens")]
        public int AcceptedPredictionTokens { get; set; }

        [JsonProperty("rejected_prediction_tokens")]
        [JsonPropertyName("rejected_prediction_tokens")]
        public int RejectedPredictionTokens { get; set; }
    }
}