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
using SocialButterflAi.Data.Analysis.Entities;
using SocialButterflAi.Models.LLMIntegration;
using SocialButterflAi.Models;
using SocialButterflAi.Models.LLMIntegration.Claude.Content;

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
        public Task<BaseResponse<UploadData>> GetAnalysisAsync(
            Guid identityId,
            AnalysisType analysisType,
            Guid? analysisId,
            string? path
        );
        
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
            string? title,
            string? description = null,
            Guid? relatedMessageId = null
        );

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public  Task<BaseResponse<AnalysisData>> AnalyzeAsync<T>(
            T request
        ) where T : BaseAnalysisRequest
        ;

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<BaseResponse<AnalysisData>> AnalyzeVideoAsync(
            VideoAnalysisRequest request
        );

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<BaseResponse<AnalysisData>> AnalyzeImageAsync(
            ImageAnalysisRequest request
        );

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<BaseResponse<AnalysisData>> AnalyzeTextAsync(
            TextAnalysisRequest request
        );

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelProvider"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="Exception"></exception>
        public BaseResponse<ClaudeMessage> FormImageContent(
            string base64Media,
            MediaType mediaType
        );

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<BaseResponse<AnalysisData>> RunAiAnalyzeAsync(
            Message message,
            ModelProvider modelProvider
        );
    }
}