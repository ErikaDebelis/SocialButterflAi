using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using ChatEntity = SocialButterflAi.Data.Chat.Entities.Chat;
using MessageDto = SocialButterflAi.Models.CueCoach.Dtos.Message;
using ChatDto = SocialButterflAi.Models.CueCoach.Dtos.Chat;
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
            bool toAnalyze = false
        );

        #region FindChats
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="matchByStatement"></param>
        /// <returns></returns>
        public IEnumerable<ChatEntity> FindChats(
            Func<ChatEntity, bool> matchByStatement
        );
        #endregion
    }
}