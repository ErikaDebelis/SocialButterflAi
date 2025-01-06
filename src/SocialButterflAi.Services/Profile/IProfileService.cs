using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using IdentityEntity = SocialButterflAi.Data.Identity.Entities.Identity;
using ProfileDto = SocialButterflAi.Models.Dtos.Profile;
using IdentityDto = SocialButterflAi.Models.Dtos.Identity;
using ProfileEntity = SocialButterflAi.Data.Identity.Entities.Profile;
using SocialButterflAi.Models;
using SocialButterflAi.Models.CueCoach;

namespace SocialButterflAi.Services.Profile
{
    public interface IProfileService
    {
        #region FindIdentities
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="matchByStatement"></param>
        /// <returns></returns>
        public IEnumerable<IdentityEntity> FindIdentities(
            Func<IdentityEntity, bool> matchByStatement
        );
        #endregion
    }
}