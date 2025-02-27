using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialButterflAi.Data.Analysis;
using SocialButterflAi.Data.Chat;
using SocialButterflAi.Data.Chat.Entities;
using SocialButterflAi.Data.Identity;
using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Services.Helpers;
using SocialButterflAi.Services.Mappers;

namespace SocialButterflAi.Services.Analysis
{
    public class Helpers
    {
        private ILogger Logger;
        private Serilog.ILogger SerilogLogger;
        private IDbQueries DbQueries;
        public Helpers(
            ILogger logger
        )
        {
            Logger = logger;
            SerilogLogger = Serilog.Log.Logger;
        }

        /// <summary>
        /// Saves an entity to the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> SaveEntity<TDto, TEntity>(
            DbContext dbContext,
            TDto dto,
            IMapper<TDto, TEntity> mapper
        ) where TDto : BaseDto
          where TEntity : BaseEntity
        {
            try
            {
                var entity = mapper.MapToEntity(dto);
                dbContext.Set<TEntity>().Add(entity);
                await dbContext.SaveChangesAsync();

                Logger.LogInformation("Entity saved successfully");
                SerilogLogger.Information("Entity saved successfully");

                return true;
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Error saving entity");
                SerilogLogger.Error(ex, "Error saving entity");
                return false;
            }
        }

                /// <summary>
        /// Saves an entity to the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> DeleteEntity<TDto, TEntity>(
            DbContext dbContext,
            TDto dto,
            IMapper<TDto, TEntity> mapper
        ) where TDto : BaseDto
          where TEntity : BaseEntity
        {
            try
            {
                var typedDbContext = dbContext.Set<TEntity>();

                var dbQueries = typedDbContext.GetType().Name switch
                {
                    nameof(AnalysisDbContext) => AnalysisDbQueries.Use(typedDbContext as AnalysisDbContext, Logger),
                    nameof(ChatDbContext) => ChatDbQueries.Use(typedDbContext as ChatDbContext, Logger),
                    nameof(IdentityDbContext) => IdentityDbQueries.Use(typedDbContext as IdentityDbContext, Logger),
                    _ => null
                };

                var foundEntity = dbQueries.FindEntities<TEntity>(entity => entity.Id == dto.Id).ToArray().FirstOrDefault();
                if (foundEntity == null)
                {
                    Logger.LogError("Entity not found");
                    SerilogLogger.Error("Entity not found");
                    return false;
                }

                typedDbContext.Remove(foundEntity);
                await dbContext.SaveChangesAsync();

                Logger.LogInformation("Entity deleted successfully");
                SerilogLogger.Information("Entity deleted successfully");
                return true;
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Error deleting entity");
                SerilogLogger.Error(ex, "Error deleting entity");
                return false;
            }
        }
    }
}