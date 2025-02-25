using System;
using System.Collections;
using System.Collections.Generic;
using SocialButterflAi.Data.Identity;

namespace SocialButterflAi.Services.Helpers
{
    public interface IDbQueries
    {
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