using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using SocialButterflAi.Data.Chat;
using SocialButterflAi.Data.Identity;

using SocialButterflAi.Services.Analysis;
using ChatEntity = SocialButterflAi.Data.Chat.Entities.Chat;

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

        #region NewMethodAsync
        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<object> ProcessMessageAsync(
        )
        {
            var response = new object();
            try
            {
                //save msg to db(need to publish db packages)
                //analyze msg (optional)
                //return response to caller(Consumer)
                throw new NotImplementedException();
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
        
        #endregion
    }
}