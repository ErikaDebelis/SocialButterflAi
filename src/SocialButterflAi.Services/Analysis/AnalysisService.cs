using System.Text;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

using SocialButterflAi.Models.Claude;
using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Models.OpenAi.Whisper;

namespace SocialButterflAi.Services.Analysis
{
    public class AnalysisService : IAnalysisService
    {
        public IOpenAiClient OpenAiClient;
        public IClaudeClient ClaudeClient;
        public ILogger<IAnalysisService> Logger;

        public AnalysisService(
            IOpenAiClient openAiClient,
            IClaudeClient claudeClient,
            ILogger<IAnalysisService> logger
        )
        {
            OpenAiClient = openAiClient;
            ClaudeClient = claudeClient;
            Logger = logger;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<AnalysisResponse> AnalyzeAsync(
            AnalysisRequest request
        )
        {
            var response = new AnalysisResponse();
            try
            {
                var whisperRequest = new WhisperRequest
                {
                    AudioFormat = AudioFormat.wav,
                    Model = Model.Whisper_1,
                    WavUrl = $"data:audio/wav;base64,{request.Base64Audio}"
                };

                var whisperResponse = await OpenAiClient.ExecuteWhisperAsync(whisperRequest);

                if(whisperResponse == null
                || !whisperResponse.Success)
                {
                    Logger.LogError("Whisper failed");

                    response.Success = false;
                    response.Message = "Whisper failed";

                    return response;
                }

                if(string.IsNullOrWhiteSpace(whisperResponse.Text))
                {
                    Logger.LogError("Whisper text is empty");

                    response.Success = false;
                    response.Message = "Whisper text is empty";

                    return response;
                }

                var message = new Message
                {
                    Content = whisperResponse.Text,
                    User = Role.User
                };

                //now that we have the audio text, we can send it to Claude for analysis
                var claudeRequest = new ClaudeRequest
                {
                    Messages = new List<Message> { message }
                };

                var claudeResponse = await ClaudeClient.AiExecutionAsync(claudeRequest);

                if(claudeResponse == null
                    || !claudeResponse.Success
                )
                {
                    Logger.LogError("Claude failed");

                    response.Success = false;
                    response.Message = "Claude failed";
                    response.Transcript = whisperResponse.Text;

                    return response;
                }

                Logger.LogInformation("Analysis completed");

                response.Success = whisperRequest.Success && claudeRequest.Success;
                response.Message = whisperRequest.Message + claudeRequest.Message;
                response.Transcript = whisperResponse.Text;
                response.Conclusion = claudeResponse.Conclusion;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
    }
}