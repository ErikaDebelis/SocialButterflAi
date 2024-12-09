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
        public Task<UploadResponse> UploadAsync(
            IFormFile file,
            VideoFormat format
        );

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<AnalysisResponse> AnalyzeAsync(
            AnalysisDtoRequest request
        );
    }
}