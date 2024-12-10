using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SocialButterflAi.Models.Integration;
using SocialButterflAi.Models.LLMIntegration;
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

        readonly Serilog.ILogger SeriLogger;

        private const string _authHeaderKey = "Authorization";
        private const string _authHeaderValuePrefix = "Bearer";
        private const string _contentType = "application/json";

        public OpenAiClient(
            OpenAiSettings settings,
            ILogger<OpenAiClient> logger
        )
        {
            _settings = settings;

            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add(_authHeaderKey, $"{_authHeaderValuePrefix} {_settings.ApiKey}");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_contentType));
            _httpClient.BaseAddress = new Uri(_settings.Url);

            SeriLogger = Serilog.Log.Logger;
            Logger = logger;
        }

        #region AiExecution
        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseAiResponse<object>> AiExecutionAsync<OpenAiRequest>(
            AiRequest<OpenAiRequest> request
        ) where OpenAiRequest : BaseAiRequestRequirements
        {
            var response = new BaseAiResponse<object>();
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

                Logger.LogInformation("response received and deserialized");
                SeriLogger.Information("response received and deserialized");;
                response.Success = true;
                response.Message = "Success";
                // response.Data = deserializedOpenAiResponse;

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