using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration
{
    public interface IBaseAiResponseRequirements
    {
        public string Id { get; set; }
    }
}