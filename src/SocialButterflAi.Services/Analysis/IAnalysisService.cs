using System;
using System.Text;
using Newtonsoft.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using SocialButterflAi.Models.LLMIntegration.Claude;
using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Models.LLMIntegration.OpenAi.Whisper;
using Microsoft.AspNetCore.Http;
using SocialButterflAi.Models.LLMIntegration;

namespace SocialButterflAi.Services.Analysis
{
    public interface IAnalysisService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="Exception"></exception>
        public Task<BaseResponse<UploadData>> UploadAsync(
            Guid identityId,
            IFormFile file,
            VideoFormat format,
            Guid? relatedChatId,
            string? title,
            string? description
        );

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<BaseResponse<AnalysisData>> AnalyzeAsync(
            AnalysisDtoRequest request,
            ModelProvider modelProvider
        );
    }
}