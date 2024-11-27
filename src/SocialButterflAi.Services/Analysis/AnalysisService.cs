using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using SocialButterflAi.Services.OpenAi;
using SocialButterflAi.Services.Claude;

using SocialButterflAi.Models.Claude;
using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Models.OpenAi.Whisper;
using Model = SocialButterflAi.Models.OpenAi.Whisper.Model;

namespace SocialButterflAi.Services.Analysis
{
    public class AnalysisService : IAnalysisService
    {
        private OpenAiClient OpenAiClient;
        private ClaudeClient ClaudeClient;
        private ILogger<IAnalysisService> Logger;

        // private readonly IWebHostEnvironment _webHostEnvironment;
        // private readonly MediaProcessor _mediaProcessor;
        private readonly string _uploadDirectory;
        private readonly string _processedDirectory;

        public AnalysisService(
            OpenAiClient openAiClient,
            ClaudeClient claudeClient,
            ILogger<IAnalysisService> logger

            // IWebHostEnvironment webHostEnvironment
        )
        {
            OpenAiClient = openAiClient;
            ClaudeClient = claudeClient;
            Logger = logger;

            // _webHostEnvironment = webHostEnvironment;
            // _mediaProcessor = new MediaProcessor();

            // Set up directories
            // _uploadDirectory = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", "Videos");
            // _processedDirectory = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", "Processed");

            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }

            if (!Directory.Exists(_processedDirectory))
            {
                Directory.CreateDirectory(_processedDirectory);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<UploadResponse> UploadAsync(
            // IFormFile file,
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
                    // await file.CopyToAsync(stream);
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

                if (string.IsNullOrEmpty(request.EndTime))
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = request.VideoPath,
                        Arguments = $"-i \"{request.VideoPath}\" 2>&1",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (var process = new Process { StartInfo = startInfo })
                    {
                        process.Start();
                        string output = await process.StandardError.ReadToEndAsync();
                        await process.WaitForExitAsync();

                        // Extract duration from FFmpeg output using regex
                        var durationMatch = Regex.Match(output, @"Duration: (\d{2}:\d{2}:\d{2})");

                        if (!durationMatch.Success)
                        {
                            throw new Exception("Could not determine video duration");
                        }

                        request.EndTime = durationMatch.Groups[1].Value;
                    }

                    Console.WriteLine($"Video timeframe: {request.EndTime} - {request.EndTime}");

                }

                string outputAudio = $"{request.VideoPath.Split('.')[0]}-{Guid.NewGuid()}.wav";
                string outputGif = $"{request.VideoPath.Split('.')[0]}-{Guid.NewGuid()}.gif";

                var processVideoResponse = await ProcessVideoFile(
                                                    request.VideoPath,
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
                var claudeRequest = new ClaudeRequest();

                claudeRequest.Messages = claudeRequest.Messages.Append(message);

                var claudeResponse = await ClaudeClient.AiExecutionAsync(claudeRequest);

                if(claudeResponse == null)
                {
                    Logger.LogError("Claude failed");

                    response.Success = false;
                    response.Message = "Claude failed";
                    response.Transcript = whisperResponse.Text;

                    return response;
                }

                Logger.LogInformation("Analysis completed");

                response.Success = whisperResponse.Success;
                response.Message = whisperResponse.Message;
                response.Transcript = whisperResponse.Text;
                response.Conclusion = claudeResponse.Content.FirstOrDefault().Text;

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
                    FileName = "", //_ffmpegPath,
                    Arguments = "" ,//$"-i \"{inputPath}\" -vn -acodec pcm_s16le -ar 44100 -ac 2 \"{outputPath}\"",
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
        private async Task<BaseResponse> CreateSynchronizedGif(
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
                    FileName = "", //_ffmpegPath,
                    Arguments = "" ,//$"-i \"{inputPath}\" -vf \"fps=10,scale=320:-1:flags=lanczos,split[s0][s1];[s0]palettegen[p];[s1][p]paletteuse\" \"{outputPath}\"",
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