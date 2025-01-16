using System;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;

using SocialButterflAi.Models.IntegrationSettings;
using SocialButterflAi.Models.LLMIntegration.HttpAbstractions;
using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Models;

namespace SocialButterflAi.Services.LLMIntegration
{
    public class TypedAiResponseHelper
    {
        private ILogger<TypedAiResponseHelper> Logger;
        readonly Serilog.ILogger SeriLogger;

        public TypedAiResponseHelper(
            ILoggerFactory loggerFactory
        )
        {
            Logger = loggerFactory.CreateLogger<TypedAiResponseHelper>();
            SeriLogger = Serilog.Log.Logger;
        }

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<AnalysisData> DeserializeResponse(
            string textContent
        )
        {
            var response = new BaseResponse<AnalysisData>();
            try
            {
                var deserializeContent = System.Text.Json.JsonSerializer.Deserialize<AnalysisData>(textContent);

                if(deserializeContent is null)
                {
                    response.Success = false;
                    response.Message = "Failed to deserialize response content";
                    return response;
                }

                response.Success = true;
                response.Message = "Successfully deserialized response content";
                response.Data = deserializeContent;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error- {ex.Message}");
                SeriLogger.Error($"Error- {ex.Message}");
                throw;
            }
        }
        #endregion
    }
}