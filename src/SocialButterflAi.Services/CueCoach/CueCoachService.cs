using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

// using CC.Data.Chat;

using SocialButterflAi.Services.Analysis;

namespace SocialButterflAi.Services.CueCoach
{
    public class CueCoachService : ICueCoachService
    {
        #region Properties (public and private)
        public IAnalysisService AnalysisService;
        // private ChatDbContext ChatDbContext;
        private ILogger<ICueCoachService> Logger;
        readonly Serilog.ILogger SeriLogger;
        #endregion

        #region Constructor
        public CueCoachService(
            IAnalysisService analysisService,
            // ChatDbContext chatDbContext,
            ILogger<ICueCoachService> logger
        )
        {
            AnalysisService = analysisService;
            // ChatDbContext = chatDbContext;
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
    }
}