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
        public Task<BaseAiResponse<T2>> AiExecutionAsync<T1, T2>(
            AiRequest<T1> request
        ) where T1 : BaseAiRequestRequirements
        where T2 : BaseAiResponseRequirements;
    }
}