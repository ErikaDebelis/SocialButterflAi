using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SocialButterflAi.Models.Claude;
using SocialButterflAi.Models.Integration;

namespace SocialButterflAi.Services.Claude
{
    public class ClaudeClient
    {
        private HttpClient _httpClient;
        private ILogger<ClaudeClient> Logger;
        private ClaudeSettings _settings;
        readonly Serilog.ILogger SeriLogger;

        private const string _authHeaderApiKeyPrefix = "x-api-key";
        private const string _contentType = "application/json";

        public ClaudeClient(
            ClaudeSettings settings,
            ILogger<ClaudeClient> logger
        )
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add(_authHeaderApiKeyPrefix, _settings.ApiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_contentType));
            _httpClient.BaseAddress = new Uri(_settings.Url);

            Logger = logger;
            SeriLogger = Serilog.Log.Logger;
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
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, _contentType);
                var claudeResponse = await _httpClient.PostAsync(_settings.Url, content);

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