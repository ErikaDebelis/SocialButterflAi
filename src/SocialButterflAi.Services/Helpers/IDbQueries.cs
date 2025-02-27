using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialButterflAi.Data.Identity;

namespace SocialButterflAi.Services.Helpers
{
    public interface IDbQueries
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        static IDbQueries Use(
            DbContext dbContext,
            ILogger logger
        ) => throw new NotImplementedException();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matchByStatement"></param>
        /// <param name="asNoTracking"></param>
        /// <returns></returns>
        IEnumerable<T> FindEntities<T>(
            Func<T, bool> matchByStatement,
            bool asNoTracking = false
        ) where T : BaseEntity;
    }
}