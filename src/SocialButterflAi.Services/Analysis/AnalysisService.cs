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
                //use ffmpeg to extract audio from video file 
                //and save it as a wav file

                //use ffmpeg to save gif from video with the same timestamp as the audio file
                // for claude to analyze the gif for microexpressions and more accurate analysis of the audio

                string inputVideo = "path/to/input/video.mp4"; //todo: update with the correct path
                string outputAudio = "path/to/output/audio.wav"; //todo: update with the correct path
                string outputGif = "path/to/output/animation.gif"; //todo: update with the correct path

                var processVideoResponse = await ProcessVideoFile(
                                                    inputVideo,
                                                    outputAudio,
                                                    outputGif
                                                );

                if(processVideoResponse == null
                    || !processVideoResponse.Success
                )
                {
                    Logger.LogError("Error processing video file");
                    response.Success = false;
                    response.Message = "Error processing video file";

                    return response;
                }

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
                var claudeRequest = new ClaudeRequest();

                claudeRequest.Messages = claudeRequest.Messages.Append(message);

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
        
        #region Private Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputVideoPath"></param>
        /// <param name="outputAudioPath"></param>
        /// <param name="outputGifPath"></param>
        /// <returns></returns>
        private async Task<BaseResponse> ProcessVideoFile(
            string inputVideoPath,
            string outputAudioPath,
            string outputGifPath
        )
        {
            var response = new BaseResponse();
            try
            {
                // Extract audio to WAV
                var audioResponse = await ExtractAudioToWav(inputVideoPath, outputAudioPath);

                if(audioResponse == null
                    || !audioResponse.Success
                )
                {
                    Logger.LogError("Error extracting audio");
                    response.Success = false;
                    response.Message = "Error extracting audio";

                    return response;
                }
                // Create synchronized GIF
                var gifResponse = await CreateSynchronizedGif(inputVideoPath, outputGifPath);

                if(gifResponse == null
                    || !gifResponse.Success
                )
                {
                    Logger.LogError("Error extracting GIF");
                    response.Success = false;
                    response.Message = "Error extracting GIF";

                    return response;
                }
                Console.WriteLine("Processing completed successfully!");

                response.Success = true;
                response.Message = "Processing completed successfully!";

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing media: {ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<BaseResponse> ExtractAudioToWav(
            string inputPath,
            string outputPath
        )
        {
            var response = new BaseResponse();
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = _ffmpegPath,
                    Arguments = //$"-i \"{inputPath}\" -vn -acodec pcm_s16le -ar 44100 -ac 2 \"{outputPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    await process.WaitForExitAsync();

                    if (process.ExitCode != 0)
                    {
                        Logger.LogError("Error extracting audio");
                        response.Success = false;
                        response.Message = "Error extracting audio";

                        return response;
                    }
                }
                Console.WriteLine("Audio extracted successfully!");

                response.Success = true;
                response.Message = "Audio extracted successfully!";

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error");
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task CreateSynchronizedGif(
            string inputPath,
            string outputPath
        )
        {
            var response = new BaseResponse();
            try
            {
                // FFmpeg command to create GIF with same duration as video
                // Adjust -r (framerate) and -s (size) parameters as needed
                //fps=10: Controls frame rate (adjust for smoothness vs file size)
                //scale=320:-1: Sets the width to 320 pixels and adjusts the height to maintain aspect ratio
                var startInfo = new ProcessStartInfo
                {
                    FileName = _ffmpegPath,
                    Arguments = //$"-i \"{inputPath}\" -vf \"fps=10,scale=320:-1:flags=lanczos,split[s0][s1];[s0]palettegen[p];[s1][p]paletteuse\" \"{outputPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    await process.WaitForExitAsync();

                    if (process.ExitCode != 0)
                    {
                        Logger.LogError("Error extracting GIF");
                        response.Success = false;
                        response.Message = "Error extracting GIF";

                        return response;
                    }
                }
                Console.WriteLine("GIF extracted successfully!");

                response.Success = true;
                response.Message = "GIF extracted successfully!";

                return response;
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