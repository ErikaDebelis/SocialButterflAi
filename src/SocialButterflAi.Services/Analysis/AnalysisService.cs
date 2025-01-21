#region Usings
using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

using SocialButterflAi.Data.Chat;
using SocialButterflAi.Data.Identity;
using SocialButterflAi.Data.Analysis;
using SocialButterflAi.Services.LLMIntegration;
using SocialButterflAi.Services.LLMIntegration.OpenAi;

using SocialButterflAi.Models;
using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Models.LLMIntegration;
using SocialButterflAi.Data.Analysis.Entities;
using SocialButterflAi.Models.IntegrationSettings;
using SocialButterflAi.Models.LLMIntegration.Claude;
using SocialButterflAi.Models.LLMIntegration.OpenAi;
using SocialButterflAi.Models.LLMIntegration.Claude.Content;
using SocialButterflAi.Models.LLMIntegration.OpenAi.Content;
using SocialButterflAi.Models.LLMIntegration.OpenAi.Whisper;
using SocialButterflAi.Models.LLMIntegration.OpenAi.Response;
using SocialButterflAi.Models.LLMIntegration.HttpAbstractions;
using VideoDto = SocialButterflAi.Models.Analysis.Video;
using VideoEntity = SocialButterflAi.Data.Analysis.Entities.Video;
using ImageDto = SocialButterflAi.Models.Analysis.Image;
using ImageEntity = SocialButterflAi.Data.Analysis.Entities.Image;
using AudioDto = SocialButterflAi.Models.Analysis.Audio;
using AudioEntity = SocialButterflAi.Data.Analysis.Entities.Audio;
using ChatEntity = SocialButterflAi.Data.Chat.Entities.Chat;
using MessageEntity = SocialButterflAi.Data.Chat.Entities.Message;
using AnalysisEntity = SocialButterflAi.Data.Analysis.Entities.Analysis;
using IntentEntity = SocialButterflAi.Data.Analysis.Entities.Intent;
using IntentDto = SocialButterflAi.Models.Analysis.Intent;
using ToneEntity = SocialButterflAi.Data.Analysis.Entities.Tone;
using ToneDto = SocialButterflAi.Models.Analysis.Tone;
using CaptionEntity = SocialButterflAi.Data.Analysis.Entities.EnhancedCaption;
using CaptionDto = SocialButterflAi.Models.Analysis.EnhancedCaption;

using MediaType = SocialButterflAi.Models.Analysis.MediaType;
using WhisperModel = SocialButterflAi.Models.LLMIntegration.OpenAi.Whisper.Model;
#endregion

namespace SocialButterflAi.Services.Analysis
{
    public class AnalysisService : IAnalysisService
    {
        #region Properties (public and private)
        private OpenAiClient OpenAiClient;
        private IAiClient ClaudeClient;
        private AnalysisDbContext AnalysisDbContext;
        private ChatDbContext ChatDbContext;

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
            ChatDbContext chatDbContext,
            ILogger<IAnalysisService> logger,
            IWebHostEnvironment webHostEnvironment,
            AnalysisSettings configuration
        )
        {
            OpenAiClient = openAiClient;
            ClaudeClient = claudeClient;

            AnalysisDbContext = analysisDbContext;
            ChatDbContext = chatDbContext;

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


        #region Get Analysis
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityId"></param>
        /// <param name="relatedChatId"></param>
        /// <param name="relatedMessageId"></param>
        /// <param name="base64Video"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse<IEnumerable<AnalysisData>>> GetAnalysisAsync(
            Guid identityId,
            MediaType analysisType,
            string path,
            Guid? analysisId
        )
        {
            var response = new BaseResponse<IEnumerable<AnalysisData>>();
            try
            {
                Func<Task<IEnumerable<AnalysisEntity>?>> media = analysisType switch
                {
                    MediaType.Video => async () =>
                    {
                        var video = FindVideos(x => x.IdentityId == identityId && x.Path == path).FirstOrDefault();

                        if (video == null)
                        {
                            response.Success = false;
                            response.Message = "Video not found";

                            return null;
                        }

                        if (analysisId != null)
                        {
                            var analysis = video.Captions
                                .Select(c => c.Analyses
                                .FirstOrDefault(a => a.Id == analysisId)
                            ).FirstOrDefault();

                            if (analysis == null)
                            {
                                response.Success = false;
                                response.Message = "Analysis not found";

                                return null;
                            }

                            response.Success = true;
                            response.Message = "Analysis successfully found";

                            return [ analysis ];

                        }

                        var analyses = video.Captions.SelectMany(c => c.Analyses).ToList();
                        if (analyses == null && !analyses.Any())
                        {
                            response.Success = false;
                            response.Message = "Analysis is not found";

                            return null;
                        }

                        response.Success = true;
                        response.Message = "Analysis successfully found";

                        return analyses;
                    },
                    MediaType.Image => async () =>
                    {
                        var image = FindImages(x => x.IdentityId == identityId && x.Path == path).FirstOrDefault();

                        if (image == null)
                        {
                            response.Success = false;
                            response.Message = "Image not found";
                            return null;
                        }

                        if (analysisId != null)
                        {
                            var analysis = image.Analyses.FirstOrDefault(a => a.Id == analysisId);

                            if (analysis == null)
                            {
                                response.Success = false;
                                response.Message = "Analysis not found";

                                return null;
                            }

                            response.Success = true;
                            response.Message = "Analysis successfully found";

                            return [ analysis ];
                        }

                        response.Success = true;
                        response.Message = "Analysis successfully found";
                        return image.Analyses;
                    },
                    MediaType.Audio => async () =>
                    {
                        var audio = FindAudios(x => x.IdentityId == identityId && x.Path == path).FirstOrDefault();

                        if (audio == null)
                        {
                            response.Success = false;
                            response.Message = "audio not found";

                            return null;
                        }

                        if (analysisId != null)
                        {
                            var analysis = audio.Captions
                                .Select(c => c.Analyses
                                    .FirstOrDefault(a => a.Id == analysisId)
                                ).FirstOrDefault();

                            if (analysis == null)
                            {
                                response.Success = false;
                                response.Message = "Analysis not found";

                                return null;
                            }

                            response.Success = true;
                            response.Message = "Analysis successfully found";

                            return [ analysis ];

                        }
                        var analyses = audio.Captions.SelectMany(c => c.Analyses).ToList();

                        if (analyses == null && !analyses.Any())
                        {
                            response.Success = false;
                            response.Message = "Analysis is not found";

                            return null;
                        }

                        response.Success = true;
                        response.Message = "Analysis successfully found";

                        return analyses;
                    },
                    _ => throw new NotImplementedException()
                };

                var result = await media();

                if(result is null)
                    return response;

                var analysesDtos = result.Select(a => AnalysisEntityToDto(a)).ToList();

                if (analysesDtos == null && !analysesDtos.Any())
                {
                    response.Success = false;
                    response.Message = "Analysis mapping failed";

                    return response;
                }
                return response;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Error");
                SeriLogger.Fatal(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
        #endregion

        #region Upload
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityId"></param>
        /// <param name="relatedChatId"></param>
        /// <param name="relatedMessageId"></param>
        /// <param name="base64Video"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse<UploadData>> UploadAsync(
            Guid identityId,
            Guid relatedMessageId,
            string base64Video
        )
        {
            var response = new BaseResponse<UploadData>();
            var videoDto = new VideoDto();
            try
            {
                var fileName = $"{Guid.NewGuid()}.{VideoFormat.mp4}";
                var filePath = Path.Combine(_uploadDirectory, fileName);

                videoDto = new VideoDto
                {
                    UploaderIdentityId = identityId,
                    MessageId = relatedMessageId,
                    Title = $"Video{relatedMessageId}",
                    Description = "",
                    Format = VideoFormat.mp4,
                    Url = filePath,
                    FileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read),
                    Base64 = base64Video
                };

                var durationData = await GetDuration(
                                            filePath,
                                            null,
                                            null
                                        );

                if(durationData is not { Success: true } )
                {
                    Logger.LogError("Error getting video duration");
                    SeriLogger.Error("Error getting video duration");
                    response.Success = false;
                    response.Message = "Error getting video duration";

                    return response;
                }

                videoDto.Duration = durationData.Data;

                var videoEntity = VideoDtoToEntity(videoDto);

                if (videoEntity == null)
                {
                    Logger.LogError("Error uploading video");
                    SeriLogger.Error("Error uploading video");
                    response.Success = false;
                    response.Message = "Error uploading video";

                    return response;
                }

                AnalysisDbContext.Videos.Add(videoEntity);
                await AnalysisDbContext.SaveChangesAsync();

                Logger.LogInformation("Video uploaded successfully");
                SeriLogger.Information("Video uploaded successfully");

                response.Success = true;
                response.Data.VideoPath = filePath;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Error");
                SeriLogger.Fatal(ex, "Error");
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityId"></param>
        /// <param name="file"></param>
        /// <param name="format"></param>
        /// <param name="relatedChatId"></param>
        /// <param name="videoTitle"></param>
        /// <param name="videoDescription"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse<UploadData>> UploadAsync(
            Guid identityId,
            IFormFile file,
            VideoFormat format,
            string? videoTitle,
            string? videoDescription = null,
            Guid? relatedMessageId = null
        )
        {
            var response = new BaseResponse<UploadData>();
            var videoDto = new VideoDto();
            try
            {
                var fileName = $"{videoTitle}:{Guid.NewGuid()}.{format}";
                var filePath = Path.Combine(_uploadDirectory, fileName);
                var base64 = string.Empty;

                using (var memoryStream = new MemoryStream())
                {
                    await file.OpenReadStream().CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    base64 = Convert.ToBase64String(fileBytes);
                }

                videoDto = new VideoDto
                {
                    UploaderIdentityId = identityId,
                    Title = videoTitle,
                    Description = videoDescription,
                    Format = format,
                    Url = filePath,
                    FileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read),
                    Base64 = base64,
                    MessageId = relatedMessageId
                };

                var durationData = await GetDuration(
                                            filePath,
                                            null,
                                            null
                                        );

                if(durationData is not { Success: true } )
                {
                    Logger.LogError("Error getting video duration");
                    SeriLogger.Error("Error getting video duration");
                    response.Success = false;
                    response.Message = "Error getting video duration";

                    return response;
                }

                videoDto.Duration = durationData.Data;

                var videoEntity = VideoDtoToEntity(videoDto);

                if (videoEntity == null)
                {
                    Logger.LogError("Error uploading video");
                    SeriLogger.Error("Error uploading video");
                    response.Success = false;
                    response.Message = "Error uploading video";

                    return response;
                }

                AnalysisDbContext.Videos.Add(videoEntity);
                await AnalysisDbContext.SaveChangesAsync();

                Logger.LogInformation("Video uploaded successfully");
                SeriLogger.Information("Video uploaded successfully");

                response.Success = true;
                response.Data.VideoPath = filePath;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Error");
                SeriLogger.Fatal(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
        #endregion

        #region UploadImage
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse<AnalysisData>> UploadImageAsync(
            object s
        )
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Error");
                SeriLogger.Fatal(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
        #endregion

        #region Analyze/Process
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse<AnalysisData>> AnalyzeAsync<T>(
            T request
        ) where T : BaseAnalysisRequest
        {
            var response = new BaseResponse<AnalysisData>();
            try
            {
                var analysisResponse = request switch
                {
                    VideoAnalysisRequest videoRequest => await AnalyzeVideoAsync(videoRequest),
                    ImageAnalysisRequest imageRequest => await AnalyzeImageAsync(imageRequest),
                    AudioAnalysisRequest audioRequest => await AnalyzeAudioAsync(audioRequest),
                    TextAnalysisRequest textRequest => await AnalyzeTextAsync(textRequest),
                    _ => new BaseResponse<AnalysisData>
                    {
                        Success = false,
                        Message = "request type not supported- did not perform analysis",
                        Data = null
                    }
                };

                if(analysisResponse is not { Success: true } )
                {
                    Logger.LogError("Error analyzing request");
                    SeriLogger.Error("Error analyzing request");

                    response.Success = false;
                    response.Message = "Error analyzing request";

                    return response;
                }

                SeriLogger.Information("Analysis completed");
                Logger.LogInformation("Analysis completed");

                response.Success = analysisResponse.Success;
                response.Message = analysisResponse.Message;
                response.Data = analysisResponse.Data;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "an exception occurred while performing analysis- see exception for details");
                SeriLogger.Fatal(ex, "an exception occurred while performing analysis- see exception for details");
                throw new Exception("an exception occurred while performing analysis- see exception for details", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelProvider"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse<AnalysisData>> AnalyzeVideoAsync(
            VideoAnalysisRequest request
        )
        {
            var response = new BaseResponse<AnalysisData>();
            try
            {
                var modelProvider = Enum.Parse<ModelProvider>($"{request.ModelProvider}");
                //use ffmpeg to extract audio from video file
                //and save it as a wav file

                //use ffmpeg to save gif from video with the same timestamp as the audio file
                // for claude to analyze the gif for microexpressions and more accurate analysis of the audio

                var matchingVideo = FindVideos(v =>
                    v.Id == request.VideoId
                    || ((v.Identity.Id == request.RequesterIdentityId
                        || v.Message.Chat.Members.FirstOrDefault(x => x.Id == v.Identity.Id) != null)
                        && v.Path == request.VideoPath
                    )
                ).FirstOrDefault();

                if (matchingVideo is null)
                {
                    Logger.LogError("Video not found");
                    SeriLogger.Error("Video not found");
                    response.Success = false;
                    response.Message = "Video not found";

                    return response;
                }

                var durationData = await GetDuration(
                                    request.VideoPath,
                                    request.StartTime,
                                    request.EndTime
                                );

                if(durationData is not { Success: true } )
                {
                    Logger.LogError("Error getting video duration");
                    SeriLogger.Error("Error getting video duration");
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

                if(processVideoResponse is not { Success: true } )
                {
                    Logger.LogError("Error processing video file");
                    SeriLogger.Error("Error processing video file");
                    response.Success = false;
                    response.Message = "Error processing video file";

                    return response;
                }

                var whisperRequest = new WhisperRequest
                {
                    AudioFormat = AudioFormat.wav,
                    Model = WhisperModel.Whisper_1,
                    Base64Audio = processVideoResponse.Data.Base64Audio
                    // WavUrl = $"data:audio/wav;base64,{request.Base64Audio}"
                };

                var whisperResponse = await OpenAiClient.ExecuteWhisperAsync(whisperRequest);

                if(whisperResponse is not { Success: true } )
                {
                    Logger.LogError("Whisper failed");
                    SeriLogger.Error("Whisper failed");

                    response.Success = false;
                    response.Message = "Whisper failed";

                    return response;
                }

                if(string.IsNullOrWhiteSpace(whisperResponse.Text))
                {
                    Logger.LogError("Whisper text is empty");
                    SeriLogger.Error("Whisper text is empty");

                    response.Success = false;
                    response.Message = "Whisper text is empty";

                    return response;
                }

                var analyzeImageRequest = new ImageAnalysisRequest
                {
                    ModelProvider = request.ModelProvider,
                    ImageId = matchingVideo.Id,
                    Transcript = whisperResponse.Text,
                };

                var aiResponse = await AnalyzeImageAsync(analyzeImageRequest);

                if(aiResponse is not { Success: true })
                {
                    Logger.LogError("Error running AI analysis");
                    SeriLogger.Error("Error running AI analysis");

                    response.Success = false;
                    response.Message = "Error running AI analysis";

                    return response;
                }

                response.Success = aiResponse.Success;
                response.Message = aiResponse.Message;
                response.Data = aiResponse.Data;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Error");
                SeriLogger.Fatal(ex, "Error");
                throw new Exception("Error", ex);
            }
        }

        // <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelProvider"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse<AnalysisData>> AnalyzeImageAsync(
            ImageAnalysisRequest request
        )
        {
            var response = new BaseResponse<AnalysisData>();
            try
            {
                var modelProvider = Enum.Parse<ModelProvider>($"{request.ModelProvider}");

                var parsedType =  Models.LLMIntegration.Claude.Content.MediaType.unknown;
                var base64Media = string.Empty;
                var matchingImage = FindImages(i =>
                    i.Id == request.ImageId
                    && (i.Identity.Id == request.RequesterIdentityId
                        || i.Message.Chat.Members.FirstOrDefault(x => x.Id == i.Identity.Id) != null
                    )
                ).FirstOrDefault();

                if (matchingImage == null)
                //save image to db from message
                {
                    var matchingMessage = ChatDbContext.Messages.FirstOrDefault(m => m.Id == request.MessageId);

                    if(matchingMessage == null)
                    {
                        Logger.LogError("Message not found");
                        SeriLogger.Error("Message not found");
                        response.Success = false;
                        response.Message = "Message not found";

                        return response;
                    }
                    //extract image type from base64 string
                    string imageType = null;
                    if (matchingMessage.Text.StartsWith("data:image/"))
                    {
                        var startIndex = "data:image/".Length;
                        var endIndex = matchingMessage.Text.IndexOf(';', startIndex);
                        if (endIndex > startIndex)
                        {
                            imageType = matchingMessage.Text.Substring(startIndex, endIndex - startIndex);
                        }
                    }

                    if (string.IsNullOrWhiteSpace(imageType))
                    {
                        Logger.LogError("Image type not found");
                        SeriLogger.Error("Image type not found");
                        response.Success = false;
                        response.Message = "Image type not found";

                        return response;
                    }

                    parsedType = imageType.ToLower() switch
                    {
                        "jpeg" => Models.LLMIntegration.Claude.Content.MediaType.image_jpeg,
                        "png" =>  Models.LLMIntegration.Claude.Content.MediaType.image_png,
                        "gif" =>  Models.LLMIntegration.Claude.Content.MediaType.image_gif,
                        "webp" =>  Models.LLMIntegration.Claude.Content.MediaType.image_webp,
                        _ => throw new NotImplementedException()
                    };

                    base64Media = matchingMessage.Text;
                }
                else
                {

                    parsedType = matchingImage.Type switch
                    {
                        ImageType.jpeg =>  Models.LLMIntegration.Claude.Content.MediaType.image_jpeg,
                        ImageType.png =>  Models.LLMIntegration.Claude.Content.MediaType.image_png,
                        ImageType.gif =>  Models.LLMIntegration.Claude.Content.MediaType.image_gif,
                        ImageType.webp =>  Models.LLMIntegration.Claude.Content.MediaType.image_webp,
                        _ => throw new NotImplementedException()
                    };

                    base64Media = matchingImage.Base64;
                }

                var formedMsgResult = FormImageContent(
                                        base64Media,
                                        parsedType
                                    );

                if(formedMsgResult is not { Success: true } )
                {
                    Logger.LogError("Error forming image content");
                    SeriLogger.Error("Error forming image content");

                    response.Success = false;
                    response.Message = "Error forming image content";

                    return response;
                }

                // //add transcript to the message
                if(!string.IsNullOrWhiteSpace(request.Transcript))
                {
                    formedMsgResult.Data.Content.ToList().Add(
                        new Models.LLMIntegration.Claude.Content.TextContent
                        {
                            Text = request.Transcript
                        }
                    );
                }

                var aiResponse = await RunAiAnalyzeAsync(
                                    formedMsgResult.Data,
                                    modelProvider
                                );

                if(aiResponse is not { Success: true })
                {
                    Logger.LogError("Error running AI analysis");
                    SeriLogger.Error("Error running AI analysis");

                    response.Success = false;
                    response.Message = "Error running AI analysis";

                    return response;
                }

                SeriLogger.Information("Analysis completed");
                Logger.LogInformation("Analysis completed");

                response.Success = aiResponse.Success;
                response.Message = aiResponse.Message;
                response.Data = aiResponse.Data;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Error");
                SeriLogger.Fatal(ex, "Error");
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
        public async Task<BaseResponse<AnalysisData>> AnalyzeAudioAsync(
            AudioAnalysisRequest request
        )
        {
            var response = new BaseResponse<AnalysisData>();
            try
            {
                var modelProvider = Enum.Parse<ModelProvider>($"{request.ModelProvider}");

                var matchingAudio = FindAudios(a =>
                    a.Id == request.AudioId
                    || ((a.Identity.Id == request.RequesterIdentityId
                        || a.Message.Chat.Members.FirstOrDefault(x => x.Id == a.Identity.Id) != null)
                        && a.Base64 == request.Base64Audio
                    )
                ).FirstOrDefault();

                if (matchingAudio is null)
                {
                    Logger.LogError("Audio not found");
                    SeriLogger.Error("Audio not found");
                    response.Success = false;
                    response.Message = "Audio not found";

                    return response;
                }

                var whisperRequest = new WhisperRequest
                {
                    AudioFormat = AudioFormat.wav,
                    Model = WhisperModel.Whisper_1,
                    Base64Audio = request.Base64Audio
                    // WavUrl = $"data:audio/wav;base64,{request.Base64Audio}"
                };

                var whisperResponse = await OpenAiClient.ExecuteWhisperAsync(whisperRequest);

                if(whisperResponse is not { Success: true } )
                {
                    Logger.LogError("Whisper failed");
                    SeriLogger.Error("Whisper failed");

                    response.Success = false;
                    response.Message = "Whisper failed";

                    return response;
                }

                if(string.IsNullOrWhiteSpace(whisperResponse.Text))
                {
                    Logger.LogError("Whisper text is empty");
                    SeriLogger.Error("Whisper text is empty");

                    response.Success = false;
                    response.Message = "Whisper text is empty";

                    return response;
                }

                var analyzeTextRequest = new TextAnalysisRequest
                {
                    ModelProvider = request.ModelProvider,
                    // MessageId = ,
                    Text = whisperResponse.Text,
                };

                var aiResponse = await AnalyzeTextAsync(analyzeTextRequest);

                if(aiResponse is not { Success: true })
                {
                    Logger.LogError("Error running AI analysis");
                    SeriLogger.Error("Error running AI analysis");

                    response.Success = false;
                    response.Message = "Error running AI analysis";

                    return response;
                }

                response.Success = aiResponse.Success;
                response.Message = aiResponse.Message;
                response.Data = aiResponse.Data;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Error");
                SeriLogger.Fatal(ex, "Error");
                throw new Exception("Error", ex);
            }
        }

        // <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelProvider"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse<AnalysisData>> AnalyzeTextAsync(
            TextAnalysisRequest request
        )
        {
            var response = new BaseResponse<AnalysisData>();
            try
            {
                var modelProvider = Enum.Parse<ModelProvider>($"{request.ModelProvider}");
                var matchingMessage = FindMessages(m => m.Id == request.MessageId).FirstOrDefault();

                if(matchingMessage == null)
                {
                    Logger.LogError("Message not found");
                    SeriLogger.Error("Message not found");
                    response.Success = false;
                    response.Message = "Message not found";

                    return response;
                }

                var aiMessage = new Models.LLMIntegration.Message()
                {
                    Content = matchingMessage.Text,
                    Role = Models.LLMIntegration.Role.User,
                };

                var analysisResponse = await RunAiAnalyzeAsync(
                                                aiMessage,
                                                modelProvider
                                            );

                if(analysisResponse == null
                    || !analysisResponse.Success
                )
                {
                    Logger.LogError($"failed to analyze message");
                    SeriLogger.Error($"failed to analyze message");
                    response.Success = false;
                    response.Message = $"failed to analyze message";
                    response.Data = null;

                    return response;
                }

                Logger.LogInformation($"Analysis completed");
                SeriLogger.Information($"Analysis completed");

                response.Success = analysisResponse.Success;
                response.Message = analysisResponse.Message;
                response.Data = analysisResponse.Data;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Error");
                SeriLogger.Fatal(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
        #endregion

        #endregion

        #region Private/Helper Methods

        #region Form Image Content
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelProvider"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="Exception"></exception>
        public BaseResponse<ClaudeMessage> FormImageContent(
            string base64Media,
            Models.LLMIntegration.Claude.Content.MediaType mediaType
        )
        {
            var response = new BaseResponse<ClaudeMessage>();
            try
            {
                var message = new ClaudeMessage
                {
                    Role = Role.User,
                    Content = new List<Models.LLMIntegration.Claude.Content.IContent>
                    {
                        new ImageContent
                        {
                            Source = new Source
                            {
                                Type = SourceType.base64,
                                MediaType = mediaType,
                                Data = base64Media
                            }
                        }
                    }
                };

                Logger.LogInformation("Image content formed successfully");
                SeriLogger.Information("Image content formed successfully");

                response.Success = true;
                response.Data = message;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Error");
                SeriLogger.Fatal(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
        #endregion

        #region Run Ai Analysis
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelProvider"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse<AnalysisData>> RunAiAnalyzeAsync(
            Message message,
            ModelProvider modelProvider
        )
        {
            var response = new BaseResponse<AnalysisData>();
            try
            {

                //now that we have the audio text, we can send it to Claude or openai for analysis
                Func<Task<BaseAiResponse<BaseAiResponseRequirements>>> aiResponse = modelProvider switch
                {
                    ModelProvider.Claude => async () =>
                    {
                        var claudeRequest = new AiRequest<ClaudeRequest>();
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
                        var openAiRequest = new AiRequest<OpenAiRequest>();

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

                if(runCompletion is not { Success: true } )
                {
                    Logger.LogError("Error running AI completion");
                    SeriLogger.Error("Error running AI completion");

                    response.Success = false;
                    response.Message = "Error running AI completion";

                    return response;
                }

                Logger.LogInformation("Analysis completed");
                SeriLogger.Information("Analysis completed");

                response.Success = runCompletion.Success;
                response.Message = runCompletion.Message;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Error");
                SeriLogger.Fatal(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
        #endregion

        #region Get Duration
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
                Logger.LogCritical(ex, "Error");
                SeriLogger.Fatal(ex, "Error");
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
        private async Task<BaseResponse<ProcessVideoData>> ProcessVideoFile(
            ProcessStartInfo startInfo,
            string outputAudioPath,
            string outputGifPath
        )
        {
            var response = new BaseResponse<ProcessVideoData>();
            try
            {
                // Extract audio to WAV
                var audioResponse = await ExtractAudioToWav(
                    startInfo,
                    outputAudioPath
                );

                if(audioResponse is not { Success: true } )
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
                    response.Data.Base64Audio = audioResponse.Data;
                    return response;
                }
                Console.WriteLine("Processing completed successfully!");

                response.Success = true;
                response.Message = "Processing completed successfully!";
                response.Data.Base64Audio = audioResponse.Data;
                response.Data.Base64Media = gifResponse.Data;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Error");
                SeriLogger.Fatal(ex, "Error");
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
                //wip: fix this- done during upload rn but also my need to happen outside of upload
                    //this is bc only certain parts of the video will be analyzed so we need to extract the audio for that part only


                //save the audio file as a base64 string
                // byte[] fileBytes;
                // await using (var memoryStream = new MemoryStream())
                // {
                //     await using (var fileStream = new FileStream(outputPath, FileMode.Open, FileAccess.Read))
                //     {
                //         await fileStream.CopyToAsync(memoryStream);
                //     }
                //     fileBytes = memoryStream.ToArray();
                // }

                // var base64Audio = Convert.ToBase64String(fileBytes);
                Console.WriteLine("Audio extracted successfully!");

                response.Success = true;
                response.Message = "Audio extracted successfully!";

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Error");
                SeriLogger.Fatal(ex, "Error");
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
                startInfo.ArgumentList.Add($"-vf fps=10, scale=320:-1, -an {outputPath}");
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

                var gifBase64 = Convert.ToBase64String(File.ReadAllBytes(outputPath));

                response.Success = true;
                response.Message = "GIF extracted successfully!";
                response.Data = gifBase64;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Error");
                SeriLogger.Fatal(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
        #endregion

        #endregion

        #region Entity/Database Methods

        #region FindAnalysis
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="matchByStatement"></param>
        /// <returns></returns>
        private IEnumerable<AnalysisEntity> FindAnalyses(
            Func<AnalysisEntity, bool> matchByStatement
        )
            => AnalysisDbContext
                .Analyses
                .Include(a => a.Caption)
                    .ThenInclude(v => v.Video)
                .Include(a => a.Caption)
                    .ThenInclude(v => v.Audio)
                .Include(a => a.Intent)
                .Include(a => a.Tone)
                .Where(matchByStatement)
                .ToArray();
        #endregion

        #region FindVideos
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="matchByStatement"></param>
        /// <returns></returns>
        private IEnumerable<VideoEntity> FindVideos(
            Func<VideoEntity, bool> matchByStatement
        )
            => AnalysisDbContext
                .Videos
                .Include(v => v.Identity)
                .Include(v => v.Message)
                    .ThenInclude(v => v.Chat)
                        .ThenInclude(m => m.Members)
                .Include(v => v.Captions)
                    .ThenInclude(c => c.Analyses)
                        .ThenInclude(a => a.Intent)
                .Include(v => v.Captions)
                    .ThenInclude(c => c.Analyses)
                        .ThenInclude(a => a.Tone)
                .Where(matchByStatement)
                .ToArray();
        #endregion

        #region FindImages
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="matchByStatement"></param>
        /// <returns></returns>
        private IEnumerable<ImageEntity> FindImages(
            Func<ImageEntity, bool> matchByStatement
        )
            => AnalysisDbContext
                .Images
                .Include(i => i.Identity)
                .Include(i => i.Message)
                    .ThenInclude(i => i.Chat)
                        .ThenInclude(m => m.Members)
                .Include(i => i.Analyses)
                    .ThenInclude(a => a.Intent)
                .Include(i => i.Analyses)
                    .ThenInclude(a => a.Tone)
                .Where(matchByStatement)
                .ToArray();
        #endregion

        #region FindAudios
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="matchByStatement"></param>
        /// <returns></returns>
        private IEnumerable<AudioEntity> FindAudios(
            Func<AudioEntity, bool> matchByStatement
        )
            => AnalysisDbContext
                .Audios
                .Include(i => i.Identity)
                .Include(i => i.Message)
                    .ThenInclude(i => i.Chat)
                        .ThenInclude(m => m.Members)
                .Include(v => v.Captions)
                    .ThenInclude(c => c.Analyses)
                        .ThenInclude(a => a.Intent)
                .Include(v => v.Captions)
                    .ThenInclude(c => c.Analyses)
                        .ThenInclude(a => a.Tone)
                .Where(matchByStatement)
                .ToArray();
        #endregion

        #region FindChats
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="matchByStatement"></param>
        /// <returns></returns>
        public IEnumerable<ChatEntity> FindChats(
            Func<ChatEntity, bool> matchByStatement
        )
            => ChatDbContext
                .Chats
                .Include(m => m.Members)
                .Include(v => v.Messages)
                    .ThenInclude(ti => ti.FromIdentity)
                .Include(v => v.Messages)
                    .ThenInclude(ti => ti.ToIdentity)
                .Where(matchByStatement)
                .ToArray();
        #endregion

        #region FindMessages
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="matchByStatement"></param>
        /// <returns></returns>
        public IEnumerable<MessageEntity> FindMessages(
            Func<MessageEntity, bool> matchByStatement
        )
            => ChatDbContext
                .Messages
                .Include(ti => ti.FromIdentity)
                .Include(ti => ti.ToIdentity)
                .Include(m => m.Chat)
                    .ThenInclude(m => m.Members)
                .Where(matchByStatement)
                .ToArray();
        #endregion

        #region SaveImageAsync
        /// <summary>
        ///
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse<ImageDto>> SaveImageAsync(
            ImageDto image
        )
        {
            var response = new BaseResponse<ImageDto>();
            try
            {
                //save to db
                var matchingImage = FindImages(c => c.Id == image.Id).FirstOrDefault();

                if(matchingImage == null)
                {
                    Logger.LogError($"Image not found for Id: {image.Id}");
                    SeriLogger.Error($"Image not found for Id: {image.Id}");
                    response.Success = false;
                    response.Message = $"Image not found for Id: {image.Id}";
                    response.Data = null;

                    return response;
                }
                var imageEntity = ImageDtoToEntity(image);

                if(imageEntity == null)
                {
                    Logger.LogError($"ImageDtoToEntity failed");
                    SeriLogger.Error($"ImageDtoToEntity failed");
                    response.Success = false;
                    response.Message = $"ImageDtoToEntity failed";
                    response.Data = null;

                    return response;
                }

                AnalysisDbContext.Images.Add(imageEntity);
                await AnalysisDbContext.SaveChangesAsync();

                Logger.LogInformation($"Image saved successfully");
                SeriLogger.Information($"Image saved successfully");
                response.Success = true;
                response.Message = "Image saved successfully";
                response.Data = image;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error");
                SeriLogger.Fatal(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
        #endregion

        #region SaveAudioAsync
        /// <summary>
        ///
        /// </summary>
        /// <param name="Audio"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse<AudioDto>> SaveAudioAsync(
            AudioDto audio
        )
        {
            var response = new BaseResponse<AudioDto>();
            try
            {
                //save to db
                var matchingAudio = FindAudios(c => c.Id == audio.Id).FirstOrDefault();

                if(matchingAudio == null)
                {
                    Logger.LogError($"Audio not found for Id: {audio.Id}");
                    SeriLogger.Error($"Audio not found for Id: {audio.Id}");
                    response.Success = false;
                    response.Message = $"Audio not found for Id: {audio.Id}";
                    response.Data = null;

                    return response;
                }
                var audioEntity = AudioDtoToEntity(audio);

                if(audioEntity == null)
                {
                    Logger.LogError($"AudioDtoToEntity failed");
                    SeriLogger.Error($"AudioDtoToEntity failed");
                    response.Success = false;
                    response.Message = $"AudioDtoToEntity failed";
                    response.Data = null;

                    return response;
                }

                AnalysisDbContext.Audios.Add(audioEntity);
                await AnalysisDbContext.SaveChangesAsync();

                Logger.LogInformation($"Audio saved successfully");
                SeriLogger.Information($"Audio saved successfully");
                response.Success = true;
                response.Message = "Audio saved successfully";
                response.Data = audio;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error");
                SeriLogger.Fatal(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
        #endregion

        #endregion

        #region Mappers

        #region VideoDtoToEntity
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="Video"> </param>
        /// <returns></returns>
        private VideoEntity VideoDtoToEntity(
            VideoDto videoDto
        )
        {
            try
            {
                var videoEntity = new VideoEntity
                {
                    Id = videoDto.Id,
                    IdentityId = videoDto.UploaderIdentityId,
                    //todo:fix this
                    // MessageId = ,
                    Title = videoDto.Title,
                    Description = videoDto.Description,
                    Path = videoDto.Url,
                    VideoType = Enum.Parse<VideoType>($"{videoDto.Format}"),
                    Base64 = videoDto.Base64,
                    Duration = videoDto.Duration.TimeSpan,
                    Captions = null,
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

        #region VideoEntityToDto
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="VideoEntity"> </param>
        /// <returns></returns>
        private async Task<VideoDto> VideoEntityToDto(
            VideoEntity videoEntity
        )
        {
            try
            {
                var videoDto = new VideoDto
                {
                    Id = videoEntity.Id,
                    UploaderIdentityId = videoEntity.IdentityId,
                    //todo:fix this
                    // MessageId = ,
                    Title = videoEntity.Title,
                    Description = videoEntity.Description,
                    Url = videoEntity.Path,
                    Format = Enum.Parse<VideoFormat>($"{videoEntity.VideoType}"),
                    Base64 = videoEntity.Base64,
                    Duration = new DurationData
                    {
                        StartTime = "00:00:00",
                        EndTime = $"{videoEntity.Duration.Hours}:{videoEntity.Duration.Minutes}:{videoEntity.Duration.Seconds}",
                        TimeSpan = videoEntity.Duration
                    },
                    FileStream = null
                };

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

        #region AnalysisDtoToEntity
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="Analysis"> </param>
        /// <returns></returns>
        private AnalysisEntity AnalysisDtoToEntity(
            AnalysisData analysisDto,
            Guid identityId
        )
        {
            try
            {
                var analysisEntity = new AnalysisEntity
                {
                    Id = analysisDto.Id ?? Guid.NewGuid(),
                    CaptionId = null,
                    Caption = null,
                    Type = Enum.Parse<Data.Analysis.Entities.MediaType>($"{analysisDto.Type}"),
                    Certainty = 0,
                    EnhancedDescription = null,
                    Tone = ToneDtoToEntity(analysisDto.Tone, identityId),
                    Intent = IntentDtoToEntity(analysisDto.Intent, identityId),
                    Metadata = null,
                    CreatedBy = $"{identityId}",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = $"{identityId}",
                    ModifiedOn = DateTime.UtcNow
                };

                if(analysisEntity == null)
                {
                    Logger.LogError($"");
                    SeriLogger.Error($"");
                    throw new Exception($"");
                }
                Logger.LogTrace($"");
                SeriLogger.Information($"");

                return analysisEntity;
            }
            catch(Exception ex)
            {
                Logger.LogCritical(ex, $"");
                SeriLogger.Fatal(ex, $"");
                throw new Exception($"", ex);
            }
        }
        #endregion

        #region AnalysisEntityToDto
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="AnalysisEntity"> </param>
        /// <returns></returns>
        private AnalysisData AnalysisEntityToDto(
            AnalysisEntity analysisEntity
        )
        {
            try
            {
                var analysisDto = new AnalysisData
                {
                    Id = analysisEntity.Id,
                    Tone = ToneEntityToDto(analysisEntity.Tone),
                    Intent = IntentEntityToDto(analysisEntity.Intent),
                    Caption = CaptionEntityToDto(analysisEntity.Caption),
                    Type = Enum.Parse<MediaType>($"{analysisEntity.Type}"),
                    Certainty = analysisEntity.Certainty,
                    EnhancedDescription = analysisEntity.EnhancedDescription,
                    Metadata = null
                };

                Logger.LogTrace($"");
                SeriLogger.Information($"");

                return analysisDto;
            }
            catch(Exception ex)
            {
                Logger.LogCritical(ex, $"");
                SeriLogger.Fatal(ex, $"");
                throw new Exception($"", ex);
            }
        }
        #endregion

        #region IntentDtoToEntity
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="Analysis"> </param>
        /// <returns></returns>
        private IntentEntity IntentDtoToEntity(
            IntentDto intentDto,
            Guid identityId
        )
        {
            try
            {
                var intentEntity = new IntentEntity
                {
                    CreatedBy = $"{identityId}",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = $"{identityId}",
                    ModifiedOn = DateTime.UtcNow
                };

                if(intentEntity == null)
                {
                    Logger.LogError($"");
                    SeriLogger.Error($"");
                    throw new Exception($"");
                }
                Logger.LogTrace($"");
                SeriLogger.Information($"");

                return intentEntity;
            }
            catch(Exception ex)
            {
                Logger.LogCritical(ex, $"");
                SeriLogger.Fatal(ex, $"");
                throw new Exception($"", ex);
            }
        }
        #endregion

        #region IntentEntityToDto
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="IntentEntity"> </param>
        /// <returns></returns>
        private IntentDto IntentEntityToDto(
            IntentEntity intentEntity
        )
        {
            try
            {
                var intentDto = new IntentDto
                {
                    Id = intentEntity.Id,
                };

                Logger.LogTrace($"");
                SeriLogger.Information($"");

                return intentDto;
            }
            catch(Exception ex)
            {
                Logger.LogCritical(ex, $"");
                SeriLogger.Fatal(ex, $"");
                throw new Exception($"", ex);
            }
        }
        #endregion

        #region ToneDtoToEntity
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="Tone"> </param>
        /// <returns></returns>
        private ToneEntity ToneDtoToEntity(
            ToneDto toneDto,
            Guid identityId
        )
        {
            try
            {
                var toneEntity = new ToneEntity
                {
                    CreatedBy = $"{identityId}",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = $"{identityId}",
                    ModifiedOn = DateTime.UtcNow
                };

                if(toneEntity == null)
                {
                    Logger.LogError($"");
                    SeriLogger.Error($"");
                    throw new Exception($"");
                }
                Logger.LogTrace($"");
                SeriLogger.Information($"");

                return toneEntity;
            }
            catch(Exception ex)
            {
                Logger.LogCritical(ex, $"");
                SeriLogger.Fatal(ex, $"");
                throw new Exception($"", ex);
            }
        }
        #endregion

        #region ToneEntityToDto
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="AnalysisEntity"> </param>
        /// <returns></returns>
        private ToneDto ToneEntityToDto(
            ToneEntity toneEntity
        )
        {
            try
            {
                var toneDto = new ToneDto
                {
                    Id = toneEntity.Id,
                };

                Logger.LogTrace($"");
                SeriLogger.Information($"");

                return toneDto;
            }
            catch(Exception ex)
            {
                Logger.LogCritical(ex, $"");
                SeriLogger.Fatal(ex, $"");
                throw new Exception($"", ex);
            }
        }
        #endregion

        #region CaptionDtoToEntity
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name=""> </param>
        /// <returns></returns>
        private CaptionEntity CaptionDtoToEntity(
            CaptionDto captionDto,
            Guid identityId
        )
        {
            try
            {
                var captionEntity = new CaptionEntity
                {
                    CreatedBy = $"{identityId}",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = $"{identityId}",
                    ModifiedOn = DateTime.UtcNow
                };

                if(captionEntity == null)
                {
                    Logger.LogError($"");
                    SeriLogger.Error($"");
                    throw new Exception($"");
                }
                Logger.LogTrace($"");
                SeriLogger.Information($"");

                return captionEntity;
            }
            catch(Exception ex)
            {
                Logger.LogCritical(ex, $"");
                SeriLogger.Fatal(ex, $"");
                throw new Exception($"", ex);
            }
        }
        #endregion

        #region CaptionEntityToDto
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name=""> </param>
        /// <returns></returns>
        private CaptionDto CaptionEntityToDto(
            CaptionEntity captionEntity
        )
        {
            try
            {
                var captionDto = new CaptionDto
                {
                    Id = captionEntity.Id,
                };

                Logger.LogTrace($"");
                SeriLogger.Information($"");

                return captionDto;
            }
            catch(Exception ex)
            {
                Logger.LogCritical(ex, $"");
                SeriLogger.Fatal(ex, $"");
                throw new Exception($"", ex);
            }
        }
        #endregion

        #region ImageDtoToEntity
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="Image"> </param>
        /// <returns></returns>
        private ImageEntity ImageDtoToEntity(
            object imageDto
        )
        {
            try
            {
                var imageEntity = new ImageEntity
                {
                    Id = default,
                    CreatedOn = default,
                    CreatedBy = null,
                    ModifiedOn = default,
                    ModifiedBy = null,
                    IdentityId = default,
                    Identity = null,
                    MessageId = null,
                    Message = null,
                    Title = null,
                    Description = null,
                    Path = null,
                    Base64 = null,
                    Type = ImageType.unknown,
                    Analyses = null
                };

                if(imageEntity == null)
                {
                    Logger.LogError($"");
                    SeriLogger.Error($"");
                    throw new Exception($"");
                }

                Logger.LogTrace($"");
                SeriLogger.Information($"");

                return imageEntity;
            }
            catch(Exception ex)
            {
                Logger.LogCritical(ex, $"");
                SeriLogger.Fatal(ex, $"");
                throw new Exception($"", ex);
            }
        }
        #endregion

        #region ImageEntityToDto
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="ImageEntity"> </param>
        /// <returns></returns>
        private async Task<object> ImageEntityToDto(
            ImageEntity imageEntity
        )
        {
            try
            {
                var imageDto = new ImageDto
                {
                    Id = default,
                    UploaderIdentityId = default,
                    MessageId = null,
                    Title = null,
                    Description = null,
                    ImageUrl = null,
                    Base64 = null
                };

                Logger.LogTrace($"");
                SeriLogger.Information($"");

                throw new NotImplementedException();
            }
            catch(Exception ex)
            {
                Logger.LogCritical(ex, $"");
                SeriLogger.Fatal(ex, $"");
                throw new Exception($"", ex);
            }
        }
        #endregion

        #region AudioDtoToEntity
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="audioDto"> </param>
        /// <returns></returns>
        private AudioEntity AudioDtoToEntity(
            object audioDto
        )
        {
            try
            {
                var audioEntity = new AudioEntity
                {
                    Id = default,
                    CreatedOn = default,
                    CreatedBy = null,
                    ModifiedOn = default,
                    ModifiedBy = null,
                    IdentityId = default,
                    Identity = null,
                    MessageId = null,
                    Message = null,
                    Base64 = null,
                    Captions = null
                };

                if(audioEntity == null)
                {
                    Logger.LogError($"");
                    SeriLogger.Error($"");
                    throw new Exception($"");
                }

                Logger.LogTrace($"");
                SeriLogger.Information($"");

                return audioEntity;
            }
            catch(Exception ex)
            {
                Logger.LogCritical(ex, $"");
                SeriLogger.Fatal(ex, $"");
                throw new Exception($"", ex);
            }
        }
        #endregion

        #region AudioEntityToDto
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="audioEntity"> </param>
        /// <returns></returns>
        private async Task<object> AudioEntityToDto(
            AudioEntity audioEntity
        )
        {
            try
            {
                var audioDto = new AudioDto
                {
                    Id = default,
                    UploaderIdentityId = default,
                    MessageId = null,
                    Base64 = null
                };

                Logger.LogTrace($"");
                SeriLogger.Information($"");

                throw new NotImplementedException();
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