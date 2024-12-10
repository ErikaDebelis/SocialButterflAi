using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using SocialButterflAi.Models.LLMIntegration.HttpAbstractions;

namespace SocialButterflAi.Models.LLMIntegration.Claude
{
    public class ClaudeRequest : BaseAiRequestRequirements
    {
        [JsonProperty("model")]
        [JsonPropertyName("model")]
        public Model Model => Model.Claude3mv5Sonnet20240620;

        [JsonProperty("system")]
        [JsonPropertyName("system")]
        public string SystemPrompt => "System"; //todo: make this meaningful

        [JsonProperty("temperature")]
        [JsonPropertyName("temperature")]
        public override float Temperature => 0.5f;

        [JsonProperty("max_tokens")]
        [JsonPropertyName("max_tokens")]
        public override int MaxTokens => 1000;

        private Message UserPrimer = new UserAnalysisPrimer().Message;
        private Message SystemPrimer = new AssistantAnalysisPrimer().Message;

        [JsonProperty("messages")]
        [JsonPropertyName("messages")]
        public IEnumerable<Message> Messages
        {
            get =>
                new List<Message>()
                {
                    UserPrimer,
                    SystemPrimer
                };
            set { }
        }
    }
}