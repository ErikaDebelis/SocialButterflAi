using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SocialButterflAi.Models.LLMIntegration.Claude;
using SocialButterflAi.Models.IntegrationSettings;
using SocialButterflAi.Models.LLMIntegration;
using SocialButterflAi.Models.LLMIntegration.HttpAbstractions;

namespace SocialButterflAi.Services.LLMIntegration.Claude
{
    public class ClaudeClient: IAiClient
    {
        private HttpClient _httpClient;
        private ILogger<ClaudeClient> Logger;
        private ClaudeSettings _settings;
        private TypedAiResponseHelper _typedAiResponseHelper;
        readonly Serilog.ILogger SeriLogger;
        private const string _authHeaderApiKeyPrefix = "x-api-key";
        private const string _contentType = "application/json";

        public ClaudeClient(
            ClaudeSettings settings,
            ILoggerFactory loggerFactory
        )
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add(_authHeaderApiKeyPrefix, _settings.ApiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_contentType));
            _httpClient.BaseAddress = new Uri(_settings.Url);

            Logger = loggerFactory.CreateLogger<ClaudeClient>();
            SeriLogger = Serilog.Log.Logger;

            _typedAiResponseHelper = new TypedAiResponseHelper(loggerFactory);
        }

        #region AiExecution
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseAiResponse<ClaudeResponse>> AiExecutionAsync<ClaudeRequest, ClaudeResponse>(
            AiRequest<ClaudeRequest> request
        ) where ClaudeRequest : BaseAiRequestRequirements
          where ClaudeResponse : BaseAiResponseRequirements
        {
            var response = new BaseAiResponse<ClaudeResponse>();
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, _contentType);
                var claudeResponse = await _httpClient.PostAsync(_settings.Url, content);

                if(!claudeResponse.IsSuccessStatusCode)
                {
                    Logger.LogError($"Error- {claudeResponse.ReasonPhrase}");
                    SeriLogger.Error($"Error- {claudeResponse.ReasonPhrase}");
                    response.Success = false;
                    response.Message = claudeResponse.ReasonPhrase;

                    return response;
                }

                var contentString = await claudeResponse.Content.ReadAsStringAsync();
                var deserializedClaudeResponse = JsonConvert.DeserializeObject<ClaudeResponse>(contentString);

                if(deserializedClaudeResponse == null)
                {
                    Logger.LogError("Error- failed to deserialize response");
                    SeriLogger.Error("Error- failed to deserialize response");
                    response.Success = false;
                    response.Message = "failed to deserialize response";

                    return response;
                }
                response.AiData = deserializedClaudeResponse;

                var deserializeContent = _typedAiResponseHelper.DeserializeResponse(contentString);

                if(deserializeContent == null
                    || !deserializeContent.Success
                    || deserializeContent.Data == null
                )
                {
                    Logger.LogWarning("failed to deserialize response- must work with untyped data");
                    SeriLogger.Error("failed to deserialize response- must work with untyped data");
                    response.Success = true;
                    response.Message = "failed to deserialize response- must work with untyped data";

                    return response;
                }

                Logger.LogInformation("response received and deserialized");
                SeriLogger.Information("response received and deserialized");;
                response.Success = true;
                response.Message = "Success";
                response.TypedData = deserializeContent.Data;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
        #endregion
    }
}