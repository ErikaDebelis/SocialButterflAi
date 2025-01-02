using System;
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
using MessageDto = SocialButterflAi.Models.CueCoach.Message;
using MessageEntity = SocialButterflAi.Data.Chat.Entities.Message;

using SocialButterflAi.Models;
using SocialButterflAi.Models.Analysis;
using Serilog;
using SocialButterflAi.Models.LLMIntegration.Claude.Content;

namespace SocialButterflAi.Services.CueCoach
{
    public class CueCoachService : ICueCoachService
    {
        #region Properties (public and private)
        public IAnalysisService AnalysisService;
        private IdentityDbContext IdentityDbContext;
        private ChatDbContext ChatDbContext;
        private ILogger<ICueCoachService> Logger;
        readonly Serilog.ILogger SeriLogger;
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
            AnalysisService = analysisService;
            IdentityDbContext = identityDbContext;
            ChatDbContext = chatDbContext;
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
            Message msg,
            Guid transactionId,
            bool toAnalyze = false
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
                        var uploadResponse = await AnalysisService.UploadAsync(
                                            identityId: msg.FromIdentityId,
                                            relatedChatId: msg.ChatId,
                                            relatedMessageId: msg.Id,
                                            base64Video: msg.Text
                                        );

                        if(uploadResponse == null
                            || !uploadResponse.Success
                        )
                        {
                            Logger.LogError($"failed to upload video");
                            SeriLogger.Error($"failed to upload video");
                            response.Success = false;
                            response.Message = $"failed to upload video";
                            response.Data.AnalysisData = null;
                        }
                        Logger.LogInformation($"Video uploaded successfully");
                        SeriLogger.Information($"Video uploaded successfully");

                        var analysisRequest = new VideoAnalysisRequest()
                        {
                            RequesterIdentityId = msg.FromIdentityId,
                            ModelProvider = $"{_modelProvider}",
                            TransactionId = $"{transactionId}",
                            VideoPath = uploadResponse.Data.VideoPath,
                            // StartTime = ,
                            // EndTime = ,
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
                    (true, MessageType.Image) => async () =>
                    {
                        var analysisRequest = new ImageAnalysisRequest()
                        {
                            RequesterIdentityId = msg.FromIdentityId,
                            ModelProvider = $"{_modelProvider}",
                            TransactionId = $"{transactionId}",
                            MessageId = msg.Id,
                            // Transcript = ,
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
                    (true, MessageType.Text) => async () =>
                    {
                        var analysisRequest = new TextAnalysisRequest()
                        {
                            RequesterIdentityId = msg.FromIdentityId,
                            ModelProvider = $"{_modelProvider}",
                            TransactionId = $"{transactionId}",
                            MessageId = msg.Id,
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
                        response.Success = false;
                        response.Message = $"no analysis requested";
                        response.Data.AnalysisData = null;

                        return false;
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

                //return response to caller(Consumer)
                Logger.LogInformation($"Message processed successfully");
                SeriLogger.Information($"Message processed successfully");
                response.Success = true;
                response.Message = "Message processed successfully";

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

        #region SaveMessageAsync
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse<Message>> SaveMessageAsync(
            Message msg
        )
        {
            var response = new BaseResponse<Message>();
            try
            {
                //save msg to db
                var matchingChat = FindChats(c => c.Id == msg.ChatId).FirstOrDefault();

                if(matchingChat == null)
                {
                    Logger.LogError($"Chat not found for ChatId: {msg.ChatId}");
                    SeriLogger.Error($"Chat not found for ChatId: {msg.ChatId}");
                    response.Success = false;
                    response.Message = $"Chat not found for ChatId: {msg.ChatId}";
                    response.Data = null;

                    return response;
                }

                var matchingMsg = FindMessages(m => m.Id == msg.Id).FirstOrDefault();

                if(matchingMsg != null)
                {
                    Logger.LogError($"Message already exists for Id: {msg.Id}- must update instead");
                    SeriLogger.Error($"Message already exists for Id: {msg.Id}- must update instead");
                    response.Success = false;
                    response.Message = $"Message already exists for Id: {msg.Id}- must update instead";
                    response.Data = null;

                    return response;
                }

                var msgEntity = MessageDtoToEntity(msg);

                if(msgEntity == null)
                {
                    Logger.LogError($"MessageDtoToEntity failed");
                    SeriLogger.Error($"MessageDtoToEntity failed");
                    response.Success = false;
                    response.Message = $"MessageDtoToEntity failed";
                    response.Data = null;

                    return response;
                }

                ChatDbContext.Messages.Add(msgEntity);
                await ChatDbContext.SaveChangesAsync();

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
                .Where(matchByStatement)
                .ToArray();
        #endregion

        #endregion

        #region Mappers

        #region MessageDtoToEntity
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="Message"> </param>
        /// <returns></returns>
        private MessageEntity MessageDtoToEntity(
            MessageDto msgDto
        )
        {
            try
            {
                var msgEntity = new MessageEntity
                {
                    Id = msgDto.Id,
                    ChatId = msgDto.ChatId,
                    FromIdentityId = msgDto.FromIdentityId,
                    ToIdentityId = msgDto.ToIdentityId,
                    MessageType = Enum.Parse<Data.Chat.Entities.MessageType>($"{msgDto.MessageType}"),
                    Metadata = msgDto.Metadata,
                    CreatedBy = $"{msgDto.FromIdentityId}",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = $"{msgDto.FromIdentityId}",
                    ModifiedOn = DateTime.UtcNow
                };

                if(msgEntity == null)
                {
                    Logger.LogError($"");
                    SeriLogger.Error($"");
                    throw new Exception($"");
                }
                Logger.LogTrace($"");
                SeriLogger.Information($"");

                return msgEntity;
            }
            catch(Exception ex)
            {
                Logger.LogCritical(ex, $"");
                SeriLogger.Fatal(ex, $"");
                throw new Exception($"", ex);
            }
        }
        #endregion

        #region MessageEntityToDto
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="msgEntity"> </param>
        /// <returns></returns>
        private async Task<MessageDto> MessageEntityToDto(
            MessageEntity msgEntity
        )
        {
            try
            {

                var msgDto = new MessageDto
                {
                    Id = msgEntity.Id,
                    ChatId = msgEntity.ChatId,
                    FromIdentityId = msgEntity.FromIdentityId,
                    ToIdentityId = msgEntity.ToIdentityId,
                    MessageType = Enum.Parse<MessageType>($"{msgEntity.MessageType}"),
                    Metadata = msgEntity.Metadata,
                };

                Logger.LogTrace($"");
                SeriLogger.Information($"");

                return msgDto;
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