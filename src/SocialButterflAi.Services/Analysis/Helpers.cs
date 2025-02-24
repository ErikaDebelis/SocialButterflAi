using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialButterflAi.Data.Identity;

namespace SocialButterflAi.Services.Analysis
{
    public class Helpers
    {
        private ILogger Logger;
        private Serilog.ILogger SerilogLogger;
        public Helpers(
            ILogger logger
        )
        {
            Logger = logger;
            SerilogLogger = Serilog.Log.Logger;
        }

        public static Helpers Use(ILogger logger) => new (logger);

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
        ) where TEntity : BaseEntity
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
    }
}