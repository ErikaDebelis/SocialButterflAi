using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using SocialButterflAi.Data.Analysis;
using AnalysisEntity = SocialButterflAi.Data.Analysis.Entities.Analysis;
using VideoEntity = SocialButterflAi.Data.Analysis.Entities.Video;
using ImageEntity = SocialButterflAi.Data.Analysis.Entities.Image;
using AudioEntity = SocialButterflAi.Data.Analysis.Entities.Audio;
using SocialButterflAi.Data.Identity;

namespace SocialButterflAi.Services.Helpers
{
    public class AnalysisDbQueries: IDbQueries
    {
        private ILogger Logger;
        private Serilog.ILogger SerilogLogger;
        private AnalysisDbContext AnalysisDbContext;
        public AnalysisDbQueries(
            AnalysisDbContext analysisDbContext,
            ILogger logger
        )
        {
            AnalysisDbContext = analysisDbContext;
            Logger = logger;
            SerilogLogger = Serilog.Log.Logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="analysisDbContext"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IDbQueries Use(
            DbContext analysisDbContext,
            ILogger logger
        )
        {
            if (analysisDbContext is AnalysisDbContext)
            {
                return new AnalysisDbQueries(analysisDbContext as AnalysisDbContext, logger);
            }

            throw new ArgumentException("Invalid DbContext type");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matchByStatement"></param>
        /// <param name="asNoTracking"></param>
        /// <returns></returns>/
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
                    nameof(AnalysisEntity) => AnalysisEntities(matchByStatement as Func<AnalysisEntity, bool>, asNoTracking) as IEnumerable<T>,
                    nameof(VideoEntity) => VideoEntities(matchByStatement as Func<VideoEntity, bool>, asNoTracking) as IEnumerable<T>,
                    nameof(ImageEntity) => ImageEntities(matchByStatement as Func<ImageEntity, bool>, asNoTracking) as IEnumerable<T>,
                    nameof(AudioEntity) => AudioEntities(matchByStatement as Func<AudioEntity, bool>, asNoTracking) as IEnumerable<T>,
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

        private AnalysisEntity[] AnalysisEntities(
            Func<AnalysisEntity, bool> matchByStatement,
            bool asNoTracking
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

        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="matchByStatement"></param>
        /// <returns></returns>
        private VideoEntity[] VideoEntities(
            Func<VideoEntity, bool> matchByStatement,
            bool asNoTracking
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


        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="matchByStatement"></param>
        /// <returns></returns>
        private ImageEntity[] ImageEntities(
            Func<ImageEntity, bool> matchByStatement,
            bool asNoTracking
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

        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="matchByStatement"></param>
        /// <returns></returns>
        private AudioEntity[] AudioEntities(
            Func<AudioEntity, bool> matchByStatement,
            bool asNoTracking
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
    }
}