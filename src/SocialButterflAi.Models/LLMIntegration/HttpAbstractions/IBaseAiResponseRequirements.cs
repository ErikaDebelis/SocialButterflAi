using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.HttpAbstractions
{
    public interface IBaseAiResponseRequirements
    {
        public string Id { get; set; }
    }
}