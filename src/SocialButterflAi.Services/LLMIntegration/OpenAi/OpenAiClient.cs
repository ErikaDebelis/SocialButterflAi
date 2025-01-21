using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SocialButterflAi.Models.IntegrationSettings;
using SocialButterflAi.Models.LLMIntegration;
using SocialButterflAi.Models.LLMIntegration.HttpAbstractions;
using SocialButterflAi.Models.LLMIntegration.OpenAi;
using SocialButterflAi.Models.LLMIntegration.OpenAi.Response;
using SocialButterflAi.Models.LLMIntegration.OpenAi.Whisper;

namespace SocialButterflAi.Services.LLMIntegration.OpenAi
{
    public class OpenAiClient: IAiClient
    {
        private HttpClient _httpClient;
        private ILogger<OpenAiClient> Logger;
        private OpenAiSettings _settings;
        private TypedAiResponseHelper _typedAiResponseHelper;
        readonly Serilog.ILogger SeriLogger;

        private const string _authHeaderKey = "Authorization";
        private const string _authHeaderValuePrefix = "Bearer";
        private const string _contentType = "application/json";

        public OpenAiClient(
            OpenAiSettings settings,
            ILoggerFactory loggerFactory
        )
        {
            _settings = settings;

            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add(_authHeaderKey, $"{_authHeaderValuePrefix} {_settings.ApiKey}");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_contentType));
            _httpClient.BaseAddress = new Uri(_settings.Url);

            SeriLogger = Serilog.Log.Logger;
            Logger = loggerFactory.CreateLogger<OpenAiClient>();
            _typedAiResponseHelper = new TypedAiResponseHelper(loggerFactory);
        }

        #region AiExecution
        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseAiResponse<OpenAiResponse>> AiExecutionAsync<OpenAiRequest, OpenAiResponse>(
            AiRequest<OpenAiRequest> request
        ) where OpenAiRequest : BaseAiRequestRequirements
          where OpenAiResponse : BaseAiResponseRequirements
        {
            var response = new BaseAiResponse<OpenAiResponse>();
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, _contentType);
                var openAiResponse = await _httpClient.PostAsync(_settings.Url, content);

                if(!openAiResponse.IsSuccessStatusCode)
                {
                    Logger.LogError($"Error- {openAiResponse.ReasonPhrase}");
                    SeriLogger.Error($"Error- {openAiResponse.ReasonPhrase}");
                    response.Success = false;
                    response.Message = openAiResponse.ReasonPhrase;

                    return response;
                }

                var contentString = await openAiResponse.Content.ReadAsStringAsync();
                var deserializedOpenAiResponse = JsonConvert.DeserializeObject<OpenAiResponse>(contentString);

                if(deserializedOpenAiResponse == null)
                {
                    Logger.LogError("Error- failed to deserialize response");
                    SeriLogger.Error("Error- failed to deserialize response");
                    response.Success = false;
                    response.Message = "failed to deserialize response";

                    return response;
                }
                response.AiData = deserializedOpenAiResponse;

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
                SeriLogger.Fatal(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
        #endregion

        #region Whisper
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<WhisperResponse> ExecuteWhisperAsync(
            WhisperRequest request
        )
        {
            var response = new WhisperResponse();
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, _contentType);
                var whisperResponse = await _httpClient.PostAsync(_settings.Url, content);

                throw new NotImplementedException();
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