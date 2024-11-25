using System.Text;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

using SocialButterflAi.Models.OpenAi.Whisper;

namespace SocialButterflAi.Services.OpenAi
{
    public class OpenAiClient
    {
        private HttpClient _httpClient;
        public ILogger<OpenAiClient> Logger;
        private Settings _settings;

        private readonly const string _authHeaderKey = "Authorization";
        private readonly const string _authHeaderValuePrefix = "Bearer";
        private readonly const string _contentType = "application/json";

        public OpenAiClient(
            Settings settings,
            ILogger<OpenAiClient> logger
        )
        {
            _settings = settings;

            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add(_authHeaderKey, $"{_authHeaderValuePrefix} {_settings.OpenAi.ApiKey}");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_contentType));
            _httpClient.BaseAddress = new Uri(_settings.OpenAi.Url);

            Logger = logger;
        }

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
                var whisperResponse = await _httpClient.PostAsync();

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