using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using SocialButterflAi.Data.Chat;
using SocialButterflAi.Data.Analysis;
using SocialButterflAi.Data.Identity;

using SocialButterflAi.Services.Mappers;
using SocialButterflAi.Services.Helpers.Db.Queries;

using SocialButterflAi.Models;

namespace SocialButterflAi.Services.Helpers.Db
{
    public class CrudHelpers
    {
        private ILogger Logger;
        private Serilog.ILogger SerilogLogger;

        public CrudHelpers(
            ILogger logger
        )
        {
            Logger = logger;
            SerilogLogger = Serilog.Log.Logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="matchByStatement"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public async Task<bool> GetEntity<TDto, TEntity>(
            DbContext dbContext,
            Func<TEntity, bool> matchByStatement,
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

                if(dbQueries == null)
                {
                    Logger.LogError("Error getting entity, dbQueries is null");
                    SerilogLogger.Error("Error getting entity, dbQueries is null");
                    return false;
                }

                var foundEntity = dbQueries.FindEntities<TEntity>(matchByStatement).ToArray().FirstOrDefault();
                if (foundEntity == null)
                {
                    Logger.LogError("Entity not found");
                    SerilogLogger.Error("Entity not found");
                    return false;
                }

                var mappedDto = mapper.MapToDto(foundEntity);

                Logger.LogInformation("Entity retrieved successfully");
                SerilogLogger.Information("Entity retrieved successfully");

                return true;
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Error getting entity");
                SerilogLogger.Error(ex, "Error getting entity");
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
        public async Task<bool> SaveEntity<TDto, TEntity>(
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

                if(dbQueries == null)
                {
                    Logger.LogError("Error deleting entity, dbQueries is null");
                    SerilogLogger.Error("Error deleting entity, dbQueries is null");
                    return false;
                }

                var foundEntity = dbQueries.FindEntities<TEntity>(entity => entity.Id == dto.Id).ToArray().FirstOrDefault();
                if (foundEntity != null)
                {
                    Logger.LogError("Entity already exists- use update instead of save");
                    SerilogLogger.Error("Entity already exists- use update instead of save");

                    return false;
                }

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
        /// 
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="dto"></param>
        /// <param name="mapper"></param>
        /// <param name="modifyingIdentityId"></param>
        /// <returns></returns>
        public async Task<bool> UpdateEntity<TDto, TEntity>(
            DbContext dbContext,
            TDto dto,
            IMapper<TDto, TEntity> mapper,
            Guid modifyingIdentityId
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

                if(dbQueries == null)
                {
                    Logger.LogError("Error getting entity, dbQueries is null");
                    SerilogLogger.Error("Error getting entity, dbQueries is null");
                    return false;
                }

                var foundEntity = dbQueries.FindEntities<TEntity>(entity => entity.Id == dto.Id).ToArray().FirstOrDefault();
                var entityType = foundEntity.GetType();

                var changedEntity = mapper.MapToEntity(dto);
                //need to check for both property and field updates

                // Exclude properties that should not be updated
                var propExclusions = typeof(BaseEntity).GetProperties().Select(p => p.Name);
                // Get all properties of the entity type that are not excluded from the update
                var props = entityType.GetProperties().Where(p => propExclusions.All(e => e != p.Name)).ToList();

                // Set the properties of the found entity to the changed entity
                props.ForEach(p =>
                {
                    try
                    {
                        var newValue = p.GetValue(changedEntity);
                        p.SetValue(foundEntity, newValue);
                    }
                    catch (Exception ex)
                    {
                        SerilogLogger.Error($"Could not set Entity property: {ex}");
                        Logger.LogError($"Could not set Entity property: {ex}");
                    }
                });

                // Exclude fields that should not be updated from the entity
                var fieldExclusions = typeof(BaseEntity).GetFields().Select(p => p.Name);
                // Get all fields of the entity type that are not excluded from the update
                var fields = entityType.GetFields().Where(p => fieldExclusions.All(e => e != p.Name)).ToList();

                // Set the fields of the found entity to the changed entity
                fields.ForEach(f =>
                {
                    try
                    {
                        var newValue = f.GetValue(changedEntity);
                        f.SetValue(foundEntity, newValue);
                    }
                    catch (Exception ex)
                    {
                        SerilogLogger.Error($"Could not set Entity field: {ex}");
                        Logger.LogError($"Could not set Entity field: {ex}");
                    }
                });

                foundEntity.ModifiedBy = $"{modifyingIdentityId}";
                foundEntity.ModifiedOn = DateTime.Now;

                await dbContext.SaveChangesAsync();

                Logger.LogInformation("Entity updated successfully");
                SerilogLogger.Information("Entity updated successfully");

                return true;
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Error updating entity");
                SerilogLogger.Error(ex, "Error updating entity");
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
            TDto dto
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

                if(dbQueries == null)
                {
                    Logger.LogError("Error deleting entity, dbQueries is null");
                    SerilogLogger.Error("Error deleting entity, dbQueries is null");
                    return false;
                }

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