using System;
using Serilog;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using SocialButterflAi.Data.Chat;
using SocialButterflAi.Data.Identity;

using SocialButterflAi.Models.CueCoach;
using SocialButterflAi.Services.Analysis;
using ChatEntity = SocialButterflAi.Data.Chat.Entities.Chat;
using MessageDto = SocialButterflAi.Models.Dtos.Message;
using ChatDto = SocialButterflAi.Models.Dtos.Chat;
using MessageEntity = SocialButterflAi.Data.Chat.Entities.Message;
using IdentityEntity = SocialButterflAi.Data.Identity.Entities.Identity;

using SocialButterflAi.Models;
using SocialButterflAi.Models.Dtos;
using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Services.Helpers.Db;
using SocialButterflAi.Services.Helpers.Db.Queries;

namespace SocialButterflAi.Services.CueCoach
{
    public class CueCoachService : ICueCoachService
    {
        #region Properties (public and private)
        private IAnalysisService AnalysisService;
        private IdentityDbContext IdentityDbContext;
        private ChatDbContext ChatDbContext;
        private ILogger<ICueCoachService> Logger;
        readonly Serilog.ILogger SeriLogger;

        private ChatDbQueries ChatDbQueries;
        private IdentityDbQueries IdentityDbQueries;
        private CrudHelpers CrudHelpers;
        private const Models.LLMIntegration.ModelProvider _modelProvider = Models.LLMIntegration.ModelProvider.Claude;
        #endregion

        #region Constructor
        public CueCoachService(
            IAnalysisService analysisService,
            IdentityDbContext identityDbContext,
            ChatDbContext chatDbContext,
            ILogger<ICueCoachService> logger
        )
        {
            CrudHelpers = new CrudHelpers(logger);
            AnalysisService = analysisService;
            IdentityDbContext = identityDbContext;
            IdentityDbQueries = new IdentityDbQueries(identityDbContext, logger);
            ChatDbContext = chatDbContext;
            ChatDbQueries = new ChatDbQueries(chatDbContext, logger);
            Logger = logger;
            SeriLogger = Serilog.Log.Logger;
        }
        #endregion

        #region Public/Main Methods

        #region ProcessMessageAsync
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="toAnalyze"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse<MessageData>> ProcessMessageAsync(
            MessageDto msg,
            Guid transactionId,
            bool toAnalyze = false,
            string? base64 = null
        )
        {
            var response = new BaseResponse<MessageData>();
            response.Data = new MessageData();
            try
            {
                //save msg to db
                var savedMsgResponse = await SaveMessageAsync(msg);

                if(savedMsgResponse == null
                    ||!savedMsgResponse.Success
                )
                {
                    Logger.LogError($"failed to save message");
                    SeriLogger.Error($"failed to save message");
                    response.Success = false;
                    response.Message = $"failed to save message";

                    return response;
                }
                response.Data.Message = savedMsgResponse.Data;

                //analyze msg (optional)

                Func<Task<bool>> analyze = (toAnalyze, msg.MessageType) switch
                {
                    (true, MessageType.Video) => async () =>
                    {
                        var videoAnalysisRequest = new VideoAnalysisRequest()
                        {
                            RequesterIdentityId = msg.FromIdentityId,
                            ModelProvider = $"{_modelProvider}",
                            TransactionId = $"{transactionId}",
                            // StartTime = ,
                            // EndTime = ,
                            // InitialUserPerception = ,
                        };

                        var uploadAndAnalysisResponse = await AnalysisService.UploadAndAnalyzeAsync<VideoAnalysisRequest>(videoAnalysisRequest, base64);

                        if(uploadAndAnalysisResponse == null
                            ||!uploadAndAnalysisResponse.Success
                        )
                        {
                            Logger.LogError($"failed to analyze message");
                            SeriLogger.Error($"failed to analyze message");
                            response.Success = false;
                            response.Message = $"failed to analyze message";
                            response.Data.AnalysisData = null;

                            return false;
                        }
                        response.Data.AnalysisData = uploadAndAnalysisResponse.Data.AnalysisData;

                        return true;
                    },
                    (true, MessageType.Image) => async () =>
                    {
                        var analysisRequest = new ImageAnalysisRequest()
                        {
                            RequesterIdentityId = msg.FromIdentityId,
                            ModelProvider = $"{_modelProvider}",
                            TransactionId = $"{transactionId}",
                            MessageId = msg.Id ?? Guid.NewGuid(),
                            // Transcript = ,
                            // InitialUserPerception = ,
                        };

                        var uploadAndAnalysisResponse = await AnalysisService.UploadAndAnalyzeAsync<ImageAnalysisRequest>(analysisRequest, base64);

                        if(uploadAndAnalysisResponse == null
                            ||!uploadAndAnalysisResponse.Success
                        )
                        {
                            Logger.LogError($"failed to analyze message");
                            SeriLogger.Error($"failed to analyze message");
                            response.Success = false;
                            response.Message = $"failed to analyze message";
                            response.Data.AnalysisData = null;

                            return false;
                        }
                        response.Data.AnalysisData = uploadAndAnalysisResponse.Data.AnalysisData;

                        return true;
                    },
                    (true, MessageType.Audio) => async () =>
                    {
                        var analysisRequest = new AudioAnalysisRequest()
                        {
                            RequesterIdentityId = msg.FromIdentityId,
                            ModelProvider = $"{_modelProvider}",
                            TransactionId = $"{transactionId}",
                            MessageId = msg.Id ?? Guid.NewGuid(),
                            // Transcript = ,
                            // InitialUserPerception = ,
                        };

                        var uploadAndAnalysisResponse = await AnalysisService.UploadAndAnalyzeAsync<AudioAnalysisRequest>(analysisRequest, base64);

                        if(uploadAndAnalysisResponse == null
                            ||!uploadAndAnalysisResponse.Success
                        )
                        {
                            Logger.LogError($"failed to analyze message");
                            SeriLogger.Error($"failed to analyze message");
                            response.Success = false;
                            response.Message = $"failed to analyze message";
                            response.Data.AnalysisData = null;

                            return false;
                        }
                        response.Data.AnalysisData = uploadAndAnalysisResponse.Data.AnalysisData;

                        return true;
                    },
                    (true, MessageType.Text) => async () =>
                    {
                        var analysisRequest = new TextAnalysisRequest()
                        {
                            RequesterIdentityId = msg.FromIdentityId,
                            ModelProvider = $"{_modelProvider}",
                            TransactionId = $"{transactionId}",
                            MessageId = msg.Id ?? Guid.NewGuid(),
                            // InitialUserPerception = ,
                        };

                        var analysisResponse = await AnalysisService.AnalyzeAsync(analysisRequest);

                        if(analysisResponse == null
                            ||!analysisResponse.Success
                        )
                        {
                            Logger.LogError($"failed to analyze message");
                            SeriLogger.Error($"failed to analyze message");
                            response.Success = false;
                            response.Message = $"failed to analyze message";
                            response.Data.AnalysisData = null;

                            return false;
                        }
                        response.Data.AnalysisData = analysisResponse.Data;

                        return true;
                    },
                    (false, _) => async () =>
                    {
                        await Task.CompletedTask;
                        Logger.LogError($"no analysis requested");
                        SeriLogger.Error($"no analysis requested");
                        response.Success = true;
                        response.Message = $"no analysis requested";
                        response.Data.AnalysisData = null;

                        return true;
                    },
                    _ => async () =>
                    {
                        await Task.CompletedTask;
                        Logger.LogError($"request fell through- unexpected values sent in for evaluation");
                        SeriLogger.Error($"request fell through- unexpected values sent in for evaluation");
                        response.Success = false;
                        response.Message = $"request fell through- unexpected values sent in for evaluation";
                        response.Data.AnalysisData = null;

                        return false;
                    }
                };

                var analysisResponse = await analyze();

                if (analysisResponse is not true)
                {
                    Logger.LogError($"request fell through- unexpected values sent in for evaluation");
                    SeriLogger.Error($"request fell through- unexpected values sent in for evaluation");
                    return response;
                }
                //return response to caller(Consumer)
                Logger.LogInformation($"Message processed successfully");
                SeriLogger.Information($"Message processed successfully");

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

        #region CreateNewChatAsync
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="toAnalyze"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse<ChatDto>> CreateNewChatAsync(
            ChatDto chat,
            Guid transactionId
        )
        {
            var response = new BaseResponse<ChatDto>();
            try
            {
                var matchingChat = ChatDbQueries.FindEntities<ChatEntity>(
                                                    c => c.Id == chat.Id
                                                ).FirstOrDefault();

                if(matchingChat != null)
                {
                    response.Success = false;
                    response.Message = $"Chat already exists";

                    Logger.LogError($"Chat already exists");
                    SeriLogger.Error($"Chat already exists");

                    response.Data = null;

                    return response;
                }

                // confirm chat members exist
                var chatMemberIds = chat.MemberIdentityIds.ToList();

                if(!chatMemberIds.Contains(chat.CreatorId))
                {
                    chatMemberIds.Add(chat.CreatorId);
                }

                chatMemberIds.ForEach(memberId =>
                {
                    var matchingIdentity = IdentityDbQueries.FindEntities<IdentityEntity>(
                                                    i => i.Id == memberId
                                                ).FirstOrDefault();

                    if(matchingIdentity == null)
                    {
                        Logger.LogError($"Identity not found for member: {memberId}. Removing from chat.");
                        SeriLogger.Error($"Identity not found for member: {memberId}. Removing from chat.");

                        chatMemberIds.Remove(memberId);
                    }
                });

                chat.MemberIdentityIds = chatMemberIds;

                //save chat to db
                var savedChatResponse = await SaveChatAsync(chat, chat.CreatorId);

                if(savedChatResponse == null
                    ||!savedChatResponse.Success
                )
                {
                    Logger.LogError($"failed to save chat");
                    SeriLogger.Error($"failed to save chat");
                    response.Success = false;
                    response.Message = $"failed to save chat";

                    return response;
                }

                response.Success = true;
                response.Message = "Chat saved successfully";

                Logger.LogInformation($"Chat saved successfully");
                SeriLogger.Information($"Chat saved successfully");

                response.Data = savedChatResponse.Data;

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

        #region Entity/Database Methods

        #region SaveChatAsync
        /// <summary>
        ///
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse<ChatDto>> SaveChatAsync(
            ChatDto chat,
            Guid creatorIdentityId
        )
        {
            var ChatMapper = new Mappers.ChatMapper();
            var response = new BaseResponse<ChatDto>();
            try
            {
                //save chat to db
                var result = await CrudHelpers.SaveEntity(
                                        ChatDbContext,
                                        chat,
                                        ChatMapper
                                    );


                Logger.LogInformation($"Chat saved successfully");
                SeriLogger.Information($"Chat saved successfully");
                response.Success = true;
                response.Message = "Chat saved successfully";
                response.Data = chat;

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

        #region SaveMessageAsync
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse<MessageDto>> SaveMessageAsync(
            MessageDto msg
        )
        {
            var ChatMapper = new Mappers.ChatMapper();
            var MessageMapper = new Mappers.MessageMapper();
            var response = new BaseResponse<MessageDto>();
            try
            {
                Func<ChatEntity, bool> matchChatByStatement = c => c.Id == msg.ChatId;

                var foundChatResult = await CrudHelpers.GetEntity(
                                        ChatDbContext,
                                        matchChatByStatement,
                                        ChatMapper
                                    );

                if(!foundChatResult)
                {
                    Logger.LogError($"Chat not found");
                    SeriLogger.Error($"Chat not found");
                    response.Success = false;
                    response.Message = $"Chat not found";
                    response.Data = null;

                    return response;
                }

                var result = await CrudHelpers.SaveEntity(
                                        ChatDbContext,
                                        msg,
                                        MessageMapper
                                    );

                if(!result)
                {
                    Logger.LogError($"MessageDtoToEntity failed");
                    SeriLogger.Error($"MessageDtoToEntity failed");
                    response.Success = false;
                    response.Message = $"MessageDtoToEntity failed";
                    response.Data = null;

                    return response;
                }

                Logger.LogInformation($"Message saved successfully");
                SeriLogger.Information($"Message saved successfully");
                response.Success = true;
                response.Message = "Message saved successfully";
                response.Data = msg;

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
    }
}