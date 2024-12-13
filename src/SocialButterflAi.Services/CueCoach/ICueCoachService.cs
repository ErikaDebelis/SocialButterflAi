using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using ChatEntity = SocialButterflAi.Data.Chat.Entities.Chat;

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
        public Task<object> ProcessMessageAsync(
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