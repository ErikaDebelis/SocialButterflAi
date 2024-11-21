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

        public ClaudeClient(ILogger<ClaudeClient> logger)
        {
            _httpClient = new HttpClient();
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