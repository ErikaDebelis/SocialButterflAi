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
using SocialButterflAi.Models;

namespace SocialButterflAi.Services.Analysis
{
    public interface IAnalysisService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityId"></param>
        /// <param name="relatedChatId"></param>
        /// <param name="relatedMessageId"></param>
        /// <param name="base64Video"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<BaseResponse<UploadData>> UploadAsync(
            Guid identityId,
            Guid relatedChatId,
            Guid relatedMessageId,
            string base64Video
        );

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