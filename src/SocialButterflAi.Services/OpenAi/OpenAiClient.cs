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

                //endpoints :
                // /v1/audio/transcriptions:	whisper-1
                // /v1/audio/translations:	whisper-1
                // /v1/audio/speech:	tts-1,  tts-1-hd (if i wanted to use the text to speech model )
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