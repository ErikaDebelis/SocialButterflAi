using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using SocialButterflAi.Data.Chat;
using ChatEntity = SocialButterflAi.Data.Chat.Entities.Chat;
using MessageEntity = SocialButterflAi.Data.Chat.Entities.Message;
using SocialButterflAi.Data.Identity;

namespace SocialButterflAi.Services.Helpers
{
    public class ChatDbQueries: IDbQueries
    {
        private ILogger Logger;
        private Serilog.ILogger SerilogLogger;
        private ChatDbContext ChatDbContext;
        public ChatDbQueries(
            ChatDbContext chatDbContext,
            ILogger logger
        )
        {
            ChatDbContext = chatDbContext;
            Logger = logger;
            SerilogLogger = Serilog.Log.Logger;
        }

        public IEnumerable<T> FindEntities<T>(
            Func<T, bool> matchByStatement,
            bool asNoTracking = false
        ) where T : BaseEntity
        {
            try
            {
                // Get the type of the entity
                var entityType = typeof(T);

                var entities = entityType.Name switch
                {
                    nameof(ChatEntity) => ChatEntities(matchByStatement as Func<ChatEntity, bool>) as IEnumerable<T>,
                    nameof(MessageEntity) => MessageEntities(matchByStatement as Func<MessageEntity, bool>) as IEnumerable<T>,
                    _ => Enumerable.Empty<T>()
                };

                return entities;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                SerilogLogger.Error(ex, ex.Message);
                return Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchByStatement"></param>
        /// <returns></returns>
        private IEnumerable<ChatEntity> ChatEntities(
            Func<ChatEntity, bool> matchByStatement,
            bool asNoTracking = false
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

        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="matchByStatement"></param>
        /// <returns></returns>
        public IEnumerable<MessageEntity> MessageEntities(
            Func<MessageEntity, bool> matchByStatement,
            bool asNoTracking = false
        )
            => ChatDbContext
                .Messages
                .Include(ti => ti.FromIdentity)
                .Include(ti => ti.ToIdentity)
                .Include(m => m.Chat)
                    .ThenInclude(m => m.Members)
                .Where(matchByStatement)
                .ToArray();
    }
}