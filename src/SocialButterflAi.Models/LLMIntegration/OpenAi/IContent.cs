using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.OpenAi
{
    public interface IContent
    {
        public Type Type { get; }
    }
}