using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.LLMIntegration.OpenAi.Content
{
    public interface IContent
    {
        public ContentType Type { get; }
    }
}