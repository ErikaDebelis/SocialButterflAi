using System.Text;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

using SocialButterflAi.Models.Claude;
using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Models.OpenAi.Whisper;

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
            AnalysisRequest request
        );
    }
}