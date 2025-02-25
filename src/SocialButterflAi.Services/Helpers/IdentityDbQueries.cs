using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using SocialButterflAi.Data.Identity;
using IdentityEntity = SocialButterflAi.Data.Identity.Entities.Identity;
using ProfileEntity = SocialButterflAi.Data.Identity.Entities.Profile;

namespace SocialButterflAi.Services.Helpers
{
    public class IdentityDbQueries: IDbQueries
    {
        private ILogger Logger;
        private Serilog.ILogger SerilogLogger;
        private IdentityDbContext IdentityDbContext;
        public IdentityDbQueries(
            IdentityDbContext identityDbContext,
            ILogger logger
        )
        {
            IdentityDbContext = identityDbContext;
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
                    nameof(IdentityEntity) => IdentityEntities(matchByStatement as Func<IdentityEntity, bool>) as IEnumerable<T>,
                    nameof(ProfileEntity) => ProfileEntities(matchByStatement as Func<ProfileEntity, bool>) as IEnumerable<T>,
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
        private IEnumerable<IdentityEntity> IdentityEntities(
            Func<IdentityEntity, bool> matchByStatement,
            bool asNoTracking = false
        )
            => IdentityDbContext
                .Identities
                .Where(matchByStatement)
                .ToArray();

        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="matchByStatement"></param>
        /// <returns></returns>
        public IEnumerable<ProfileEntity> ProfileEntities(
            Func<ProfileEntity, bool> matchByStatement,
            bool asNoTracking = false
        )
            => IdentityDbContext
                .Profiles
                .Where(matchByStatement)
                .ToArray();
    }
}