using System.Text;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

using SocialButterflAi.Models.Claude;
using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Models.OpenAi.Whisper;

namespace SocialButterflAi.Services.Analysis
{
    public class AnalysisService : IAnalysisService
    {
        public ILogger<IAnalysisService> Logger;

        public AnalysisService(ILogger<IAnalysisService> logger)
        {
            Logger = logger;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<object> Analyze(
            object request
        )
        {
            var response = new object();
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
    }
}