using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Text.Json;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

// using CC.Data.Analysis;
using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Models.Integration;
using SocialButterflAi.Services.LLMIntegration;
using SocialButterflAi.Models.LLMIntegration.Claude;
using SocialButterflAi.Models.LLMIntegration.OpenAi;
using SocialButterflAi.Models.LLMIntegration.OpenAi.Whisper;
using Model = SocialButterflAi.Models.LLMIntegration.OpenAi.Whisper.Model;
using SocialButterflAi.Services.LLMIntegration.OpenAi;
using SocialButterflAi.Models.LLMIntegration;
using SocialButterflAi.Models.LLMIntegration.OpenAi.Response;

namespace SocialButterflAi.Services.Analysis
{
    public class AnalysisService : IAnalysisService
    {
        #region Properties (public and private)
        private OpenAiClient OpenAiClient;
        private IAiClient ClaudeClient;
        // private AnalysisDbContext AnalysisDbContext;

        private ILogger<IAnalysisService> Logger;
        readonly Serilog.ILogger SeriLogger;

        private readonly IWebHostEnvironment _webHostEnvironment;
        // private readonly MediaProcessor _mediaProcessor;
        private readonly string _uploadDirectory;
        private readonly string _processedDirectory;
        private readonly long _maxFileSize;
        private readonly AnalysisSettings _configuration;
        #endregion

        #region Constructor
        public AnalysisService(
            OpenAiClient openAiClient,
            IAiClient claudeClient,
            // AnalysisDbContext analysisDbContext,
            ILogger<IAnalysisService> logger,
            IWebHostEnvironment webHostEnvironment,
            AnalysisSettings configuration
        )
        {
            OpenAiClient = openAiClient;
            ClaudeClient = claudeClient;

            // AnalysisDbContext = analysisDbContext;

            Logger = logger;
            SeriLogger = Serilog.Log.Logger;

            _webHostEnvironment = webHostEnvironment;
            //_mediaProcessor = new MediaProcessor();

            _maxFileSize = long.Parse(configuration.VideoSettings.MaxFileSize);
            //Set up directories
            _uploadDirectory = Path.Combine(_webHostEnvironment.ContentRootPath, configuration.VideoSettings.UploadPath, "Videos");
            _processedDirectory = Path.Combine(_webHostEnvironment.ContentRootPath, configuration.VideoSettings.ProcessedPath, "Processed");
            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }

            if (!Directory.Exists(_processedDirectory))
            {
                Directory.CreateDirectory(_processedDirectory);
            }
        }
        #endregion

        #region Public/Main Methods

        #region Upload
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<UploadResponse> UploadAsync(
            IFormFile file,
            VideoFormat format
        )
        {
            var response = new UploadResponse();
            try
            {
                var fileName = $"{Guid.NewGuid()}.{format}";
                var filePath = Path.Combine(_uploadDirectory, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                response.Success = true;
                response.VideoPath = filePath;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
        #endregion

        #region Analyze/Process
        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<AnalysisResponse> AnalyzeAsync(
            AnalysisDtoRequest request,
            ModelProvider modelProvider
        )
        {
            var response = new AnalysisResponse();
            try
            {
                //use ffmpeg to extract audio from video file
                //and save it as a wav file

                //use ffmpeg to save gif from video with the same timestamp as the audio file
                // for claude to analyze the gif for microexpressions and more accurate analysis of the audio

                var startInfo = new ProcessStartInfo
                {
                    FileName = request.VideoPath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                if (string.IsNullOrEmpty(request.EndTime))
                {

                    using (var process = new Process { StartInfo = startInfo })
                    {
                        process.Start();
                        string output = await process.StandardError.ReadToEndAsync();
                        await process.WaitForExitAsync();

                        // Extract duration from FFmpeg output using regex
                        if(string.IsNullOrWhiteSpace(request.EndTime))
                        {
                            var durationMatch = Regex.Match(output, @"Duration: (\d{2}:\d{2}:\d{2})");

                            if (!durationMatch.Success)
                            {
                                throw new Exception("Could not determine video duration");
                            }
                            request.EndTime = durationMatch.Groups[1].Value;
                        }
                        Console.WriteLine($"Video timeframe: {request.StartTime} - {request.EndTime}");
                        // add the start and end time to the ProcessStartInfo arguments to extract the audio from the video 
                        startInfo.ArgumentList.Add($"-ss {request.StartTime}");
                        startInfo.ArgumentList.Add($"-to {request.EndTime}");
                        startInfo.ArgumentList.Add("-c copy"); // -c copy: copy codec
                    }
                }

                string outputAudio = $"{request.VideoPath.Split('.')[0]}-{Guid.NewGuid()}.wav";
                string outputGif = $"{request.VideoPath.Split('.')[0]}-{Guid.NewGuid()}.gif";

                var processVideoResponse = await ProcessVideoFile(
                                                startInfo,
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
                    // WavUrl = $"data:audio/wav;base64,{request.Base64Audio}"
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
                    Role = Role.User
                };

                //now that we have the audio text, we can send it to Claude for analysis

                Func<Task<BaseAiResponse<BaseAiResponseRequirements>>> aiResponse = modelProvider switch
                {
                    ModelProvider.Claude => async () =>
                    {
                        var claudeRequest = new AiRequest<ClaudeRequest>{ };
                        claudeRequest.AiData.Messages = claudeRequest.AiData.Messages.Append(message);

                        var claudeResponse = await ClaudeClient.AiExecutionAsync<ClaudeRequest, ClaudeResponse>(claudeRequest);

                        return new BaseAiResponse<BaseAiResponseRequirements>
                        {
                            Success = claudeResponse.Success,
                            Message = claudeResponse.Message,
                            AiData = claudeResponse.AiData
                        };
                    },
                    ModelProvider.OpenAi => async () =>
                    {
                        var openAiRequest = new AiRequest<OpenAiRequest>{ };

                        var openAiResponse = await OpenAiClient.AiExecutionAsync<OpenAiRequest, OpenAiResponse>(openAiRequest);

                        return new BaseAiResponse<BaseAiResponseRequirements>
                        {
                            Success = openAiResponse.Success,
                            Message = openAiResponse.Message,
                            AiData = openAiResponse.AiData
                        };
                    },
                    _ => throw new NotImplementedException()
                };

                var runCompletion = await aiResponse();

                if(runCompletion == null
                    || !runCompletion.Success
                )
                {
                    Logger.LogError("Error running AI completion");

                    response.Success = false;
                    response.Message = "Error running AI completion";

                    return response;
                }

                Logger.LogInformation("Analysis completed");

                response.Success = whisperResponse.Success;
                response.Message = whisperResponse.Message;
                response.Transcript = whisperResponse.Text;
                // response.Conclusion = aiResponse.Content.FirstOrDefault().Text;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
        #endregion

        #endregion

        #region Private Methods

        #region Process Video File
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputVideoPath"></param>
        /// <param name="outputAudioPath"></param>
        /// <param name="outputGifPath"></param>
        /// <returns></returns>
        private async Task<BaseResponse> ProcessVideoFile(
            ProcessStartInfo startInfo,
            string outputAudioPath,
            string outputGifPath
        )
        {
            var response = new BaseResponse();
            try
            {
                // Extract audio to WAV
                var audioResponse = await ExtractAudioToWav(
                    startInfo,
                    outputAudioPath
                );

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
                var gifResponse = await CreateSynchronizedGif(
                    startInfo,
                    outputGifPath
                );

                if(gifResponse == null)
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
                Logger.LogError(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
        #endregion

        #region Extract Audio To WAV
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<BaseResponse> ExtractAudioToWav(
            ProcessStartInfo startInfo,
            string outputPath
        )
        {
            var response = new BaseResponse();
            try
            {
                startInfo.ArgumentList.Add("-vn"); //-vn: no video

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
        #endregion

        #region Create Synchronized GIF
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<BaseResponse> CreateSynchronizedGif(
            ProcessStartInfo startInfo,
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
                //no audio - an: -an: no audio
                startInfo.ArgumentList.Add("-vf fps=10, scale=320:-1, -an");

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

        #endregion
    }
}