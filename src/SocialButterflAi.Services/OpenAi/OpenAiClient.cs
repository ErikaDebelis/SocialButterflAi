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

        public OpenAiClient(ILogger<OpenAiClient> logger)
        {
            _httpClient = new HttpClient();
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