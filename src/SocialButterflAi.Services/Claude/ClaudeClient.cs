using System.Text;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

using SocialButterflAi.Models.Claude;

namespace SocialButterflAi.Services.Claude
{
    public class ClaudeClient
    {
        private HttpClient _httpClient;
        public ILogger<ClaudeClient> Logger;
        private Settings _settings;

        private readonly const string _authHeaderApiKeyPrefix = "x-api-key";
        private readonly const string _contentType = "application/json";

        public ClaudeClient(
            Settings settings,
            ILogger<ClaudeClient> logger
        )
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add(_authHeaderApiKeyPrefix, _settings.Claude.ApiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_contentType));
            _httpClient.BaseAddress = new Uri(_settings.Claude.Url);

            Logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ClaudeResponse> AiExecutionAsync(
            ClaudeRequest request
        )
        {
            var response = new ClaudeResponse();
            try
            {
                var claudeResponse = await _httpClient.PostAsync();

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