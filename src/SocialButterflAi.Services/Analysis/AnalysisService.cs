using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

using SocialButterflAi.Data.Analysis;
using SocialButterflAi.Services.LLMIntegration;
using SocialButterflAi.Services.LLMIntegration.OpenAi;

using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Models.Integration;
using SocialButterflAi.Models.LLMIntegration;
using SocialButterflAi.Models.LLMIntegration.Claude;
using SocialButterflAi.Models.LLMIntegration.OpenAi;
using SocialButterflAi.Models.LLMIntegration.OpenAi.Whisper;
using SocialButterflAi.Models.LLMIntegration.OpenAi.Response;
using SocialButterflAi.Models.LLMIntegration.HttpAbstractions;
using Model = SocialButterflAi.Models.LLMIntegration.OpenAi.Whisper.Model;
using VideoEntity = SocialButterflAi.Data.Analysis.Entities.Video;
using Serilog;

namespace SocialButterflAi.Services.Analysis
{
    public class AnalysisService : IAnalysisService
    {
        #region Properties (public and private)
        private OpenAiClient OpenAiClient;
        private IAiClient ClaudeClient;
        private AnalysisDbContext AnalysisDbContext;

        private ILogger<IAnalysisService> Logger;
        readonly Serilog.ILogger SeriLogger;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _uploadDirectory;
        private readonly string _processedDirectory;
        private readonly long _maxFileSize;
        private readonly AnalysisSettings _configuration;
        #endregion

        #region Constructor
        public AnalysisService(
            OpenAiClient openAiClient,
            IAiClient claudeClient,
            AnalysisDbContext analysisDbContext,
            ILogger<IAnalysisService> logger,
            IWebHostEnvironment webHostEnvironment,
            AnalysisSettings configuration
        )
        {
            OpenAiClient = openAiClient;
            ClaudeClient = claudeClient;

            AnalysisDbContext = analysisDbContext;

            Logger = logger;
            SeriLogger = Serilog.Log.Logger;

            _webHostEnvironment = webHostEnvironment;

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
        public async Task<BaseResponse<UploadData>> UploadAsync(
            Guid identityId,
            IFormFile file,
            VideoFormat format,
            Guid? relatedChatId,
            string? videoTitle,
            string? videoDescription = null
        )
        {
            var response = new BaseResponse<UploadData>();
            var videoDto = new VideoDto();
            try
            {
                var fileName = $"{videoTitle}:{Guid.NewGuid()}.{format}";
                var filePath = Path.Combine(_uploadDirectory, fileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);

                    videoDto = new VideoDto
                    {
                        UploaderIdentityId = identityId,
                        RelatedChatId = relatedChatId,
                        Title = videoTitle,
                        Description = videoDescription,
                        Format = format,
                        Url = filePath,
                        FileStream = stream
                    };
                }
                var durationData = await GetDuration(
                    filePath,
                    null,
                    null
                );

                if(durationData == null
                    || !durationData.Success
                )
                {
                    Logger.LogError("Error getting video duration");
                    SeriLogger.Error("Error getting video duration");
                    response.Success = false;
                    response.Message = "Error getting video duration";

                    return response;
                }

                videoDto.Duration = durationData.Data.TimeSpan;

                if (videoDto == null)
                {
                    Logger.LogError("Error uploading video");
                    SeriLogger.Error("Error uploading video");
                    response.Success = false;
                    response.Message = "Error uploading video";

                    return response;
                }

                var videoEntity = VideoDtoToEntity(videoDto);

                Logger.LogInformation("Video uploaded successfully");
                SeriLogger.Information("Video uploaded successfully");

                response.Success = true;
                response.Data.VideoPath = filePath;

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
        public async Task<BaseResponse<AnalysisData>> AnalyzeAsync(
            AnalysisDtoRequest request,
            ModelProvider modelProvider = ModelProvider.Claude
        )
        {
            var response = new BaseResponse<AnalysisData>();
            try
            {
                //use ffmpeg to extract audio from video file
                //and save it as a wav file

                //use ffmpeg to save gif from video with the same timestamp as the audio file
                // for claude to analyze the gif for microexpressions and more accurate analysis of the audio

                var durationData = await GetDuration(
                    request.VideoPath,
                    request.StartTime,
                    request.EndTime
                );

                if(durationData == null
                    || !durationData.Success
                )
                {
                    Logger.LogError("Error getting video duration");
                    response.Success = false;
                    response.Message = "Error getting video duration";

                    return response;
                }

                var outputAudio = $"{request.VideoPath.Split('.')[0]}-{Guid.NewGuid()}.wav";
                var outputGif = $"{request.VideoPath.Split('.')[0]}-{Guid.NewGuid()}.gif";

                var processVideoResponse = await ProcessVideoFile(
                                                durationData.Data.ProcessStartInfo,
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
                    || !whisperResponse.Success
                )
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

                //now that we have the audio text, we can send it to Claude or openai for analysis
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
                response.Data.Transcript = whisperResponse.Text;
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

        #region Private/ Helper Methods

        #region Process Video File
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputVideoPath"></param>
        /// <param name="outputAudioPath"></param>
        /// <param name="outputGifPath"></param>
        /// <returns></returns>
        private async Task<BaseResponse<DurationData>> GetDuration(
            string inputVideoPath,
            string? startTime,
            string? endTime
        )
        {
            var response = new BaseResponse<DurationData>();
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = inputVideoPath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                if (!string.IsNullOrWhiteSpace(startTime))
                {
                    // Validate start time format
                    if (!Regex.IsMatch(startTime, @"\d{2}:\d{2}:\d{2}"))
                    {
                        throw new Exception("Invalid start time format");
                    }

                    Console.WriteLine($"Video start time: {startTime}");

                    // add the start time to the ProcessStartInfo arguments to extract the audio from the video
                    startInfo.ArgumentList.Add($"-ss {startTime}");
                }

                using var process = new Process { StartInfo = startInfo };
                process.Start();
                var output = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();

                if (string.IsNullOrWhiteSpace(endTime))
                {
                    // Extract duration from FFmpeg output using regex
                    var durationMatch = Regex.Match(output, @"Duration: (\d{2}:\d{2}:\d{2})");

                    if (!durationMatch.Success)
                    {
                        Logger.LogError("Could not determine video duration");
                        SeriLogger.Error("Could not determine video duration");
                        response.Success = false;
                        response.Message = "Could not determine video duration";

                        return response;
                    }
                    endTime = durationMatch.Groups[1].Value;

                    Console.WriteLine($"Video timeframe: {startTime} - {endTime}");
                    // add the start and end time to the ProcessStartInfo arguments to extract the audio from the video
                    startInfo.ArgumentList.Add($"-to {endTime}");
                }
                startInfo.ArgumentList.Add("-c copy"); // -c copy: copy codec

                Logger.LogInformation("Duration extracted successfully!");
                SeriLogger.Information("Duration extracted successfully!");

                response.Success = true;
                response.Message = "Duration extracted successfully!";
                response.Data = new DurationData
                {
                    StartTime = startTime,
                    EndTime = endTime
                };

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
        #endregion

        #region Process Video File
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputVideoPath"></param>
        /// <param name="outputAudioPath"></param>
        /// <param name="outputGifPath"></param>
        /// <returns></returns>
        private async Task<BaseResponse<string>> ProcessVideoFile(
            ProcessStartInfo startInfo,
            string outputAudioPath,
            string outputGifPath
        )
        {
            var response = new BaseResponse<string>();
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
        private async Task<BaseResponse<string>> ExtractAudioToWav(
            ProcessStartInfo startInfo,
            string outputPath
        )
        {
            var response = new BaseResponse<string>();
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
        private async Task<BaseResponse<string?>> CreateSynchronizedGif(
            ProcessStartInfo startInfo,
            string outputPath
        )
        {
            var response = new BaseResponse<string?>();
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

        #region Database Methods

        #region VideoDtoToEntity
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="Video"> </param>
        /// <returns></returns>
        public VideoEntity VideoDtoToEntity(
            VideoDto videoDto
        )
        {
            try
            {
                var videoEntity = new VideoEntity
                {
                    Id = videoDto.Id,
                    CreatedBy = $"{videoDto.UploaderIdentityId}",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = $"{videoDto.UploaderIdentityId}",
                    ModifiedOn = DateTime.UtcNow
                };

                if(videoEntity == null)
                {
                    Logger.LogError($"");
                    SeriLogger.Error($"");
                    throw new Exception($"");
                }
                Logger.LogTrace($"");
                SeriLogger.Information($"");

                return videoEntity;
            }
            catch(Exception ex)
            {
                Logger.LogCritical(ex, $"");
                SeriLogger.Fatal(ex, $"");
                throw new Exception($"", ex);
            }
        }
        #endregion

        #region 🗺️ VideoEntityToDto 🗺️
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="VideoEntity"> </param>
        /// <returns></returns>
        public VideoDto VideoEntityToDto(
            VideoEntity videoEntity
        )
        {
            try
            {

                var videoDto = new VideoDto
                {
                };

                if (videoDto == null)
                {
                    Logger.LogError($"Error mapping ");
                    SeriLogger.Error($"Error mapping ");
                    throw new Exception($"Error mapping ");
                }
                Logger.LogTrace($"");
                SeriLogger.Information($"");

                return videoDto;
            }
            catch(Exception ex)
            {
                Logger.LogCritical(ex, $"");
                SeriLogger.Fatal(ex, $"");
                throw new Exception($"", ex);
            }
        }
        #endregion

        #endregion
    }
}