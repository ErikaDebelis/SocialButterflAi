using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using ChatEntity = SocialButterflAi.Data.Chat.Entities.Chat;
using MessageDto = SocialButterflAi.Models.Dtos.Message;
using ChatDto = SocialButterflAi.Models.Dtos.Chat;
using MessageEntity = SocialButterflAi.Data.Chat.Entities.Message;
using SocialButterflAi.Models;
using SocialButterflAi.Models.CueCoach;

namespace SocialButterflAi.Services.CueCoach
{
    public interface ICueCoachService
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="Exception"></exception>
        public Task<BaseResponse<MessageData>> ProcessMessageAsync(
            MessageDto msg,
            Guid transactionId,
            bool toAnalyze = false,
            string? base64 = null
        );
    }
}