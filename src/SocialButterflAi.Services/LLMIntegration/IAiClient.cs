using System;
using System.Threading.Tasks;
using SocialButterflAi.Models.LLMIntegration;

namespace SocialButterflAi.Services.LLMIntegration
{
    public interface IAiClient
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="Exception"></exception>
        public Task<BaseAiResponse<object>> AiExecutionAsync<T>(
            AiRequest<T> request
        ) where T : BaseAiRequestRequirements;
    }
}