using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SocialButterflAi.Models.Integration;
using SocialButterflAi.Models.OpenAi.Whisper;

namespace SocialButterflAi.Services.OpenAi
{
    public class OpenAiClient
    {
        private HttpClient _httpClient;
        private ILogger<OpenAiClient> Logger;
        private OpenAiSettings _settings;

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
    }
}