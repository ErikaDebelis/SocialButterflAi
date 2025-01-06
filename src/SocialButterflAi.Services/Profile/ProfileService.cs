using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using SocialButterflAi.Data.Identity;
using SocialButterflAi.Services.Analysis;

using SocialButterflAi.Models;
using SocialButterflAi.Models.Analysis;
using IdentityEntity = SocialButterflAi.Data.Identity.Entities.Identity;
using ProfileDto = SocialButterflAi.Models.Dtos.Profile;
using IdentityDto = SocialButterflAi.Models.Dtos.Identity;
using ProfileEntity = SocialButterflAi.Data.Identity.Entities.Profile;
using Serilog;

namespace SocialButterflAi.Services.Profile
{
    public class ProfileService : IProfileService
    {
        #region Properties (public and private)
        private IdentityDbContext IdentityDbContext;
        private ILogger<IProfileService> Logger;
        readonly Serilog.ILogger SeriLogger;
        #endregion

        #region Constructor
        public ProfileService(
            IdentityDbContext identityDbContext,
            ILogger<IProfileService> logger
        )
        {
            IdentityDbContext = identityDbContext;
            Logger = logger;
            SeriLogger = Serilog.Log.Logger;
        }
        #endregion

        #region Public/Main Methods

        #region SaveIdentityAsync
        /// <summary>
        ///
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse<IdentityDto>> SaveIdentityAsync(
            IdentityDto identity,
            Guid creatorIdentityId
        )
        {
            var response = new BaseResponse<IdentityDto>();
            try
            {
                //save to db
                var matchingIdentity = FindIdentities(c => c.Id == identity.Id).FirstOrDefault();

                if(matchingIdentity == null)
                {
                    Logger.LogError($"Identity not found for Id: {identity.Id}");
                    SeriLogger.Error($"Identity not found for Id: {identity.Id}");
                    response.Success = false;
                    response.Message = $"Identity not found for Id: {identity.Id}";
                    response.Data = null;

                    return response;
                }
                var identityEntity = IdentityDtoToEntity(identity, creatorIdentityId);

                if(identityEntity == null)
                {
                    Logger.LogError($"IdentityDtoToEntity failed");
                    SeriLogger.Error($"IdentityDtoToEntity failed");
                    response.Success = false;
                    response.Message = $"IdentityDtoToEntity failed";
                    response.Data = null;

                    return response;
                }

                IdentityDbContext.Identities.Add(identityEntity);
                await IdentityDbContext.SaveChangesAsync();

                Logger.LogInformation($"Identity saved successfully");
                SeriLogger.Information($"Identity saved successfully");
                response.Success = true;
                response.Message = "Identity saved successfully";
                response.Data = identity;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error");
                SeriLogger.Fatal(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
        #endregion

        #region SaveProfileAsync
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse<ProfileDto>> SaveProfileAsync(
            ProfileDto profile
        )
        {
            var response = new BaseResponse<ProfileDto>();
            try
            {
                //save msg to db
                var matchingIdentity = FindIdentities(c => c.Id == profile.IdentityId).FirstOrDefault();

                if(matchingIdentity == null)
                {
                    Logger.LogError($"Identity not found for IdentityId: {profile.IdentityId}");
                    SeriLogger.Error($"Identity not found for IdentityId: {profile.IdentityId}");
                    response.Success = false;
                    response.Message = $"Identity not found for IdentityId: {profile.IdentityId}";
                    response.Data = null;

                    return response;
                }

                var matchingProfile = FindProfiles(m => m.Id == profile.Id).FirstOrDefault();

                if(matchingProfile != null)
                {
                    Logger.LogError($"Profile already exists for Id: {profile.Id}- must update instead");
                    SeriLogger.Error($"Profile already exists for Id: {profile.Id}- must update instead");
                    response.Success = false;
                    response.Message = $"Profile already exists for Id: {profile.Id}- must update instead";
                    response.Data = null;

                    return response;
                }

                var profileEntity = ProfileDtoToEntity(profile);

                if(profileEntity == null)
                {
                    Logger.LogError($"ProfileDtoToEntity failed");
                    SeriLogger.Error($"ProfileDtoToEntity failed");
                    response.Success = false;
                    response.Message = $"ProfileDtoToEntity failed";
                    response.Data = null;

                    return response;
                }

                IdentityDbContext.Profiles.Add(profileEntity);
                await IdentityDbContext.SaveChangesAsync();

                Logger.LogInformation($"Profile saved successfully");
                SeriLogger.Information($"Profile saved successfully");
                response.Success = true;
                response.Message = "Profile saved successfully";
                response.Data = profile;

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error");
                SeriLogger.Fatal(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
        #endregion

        #region FindIdentities
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="matchByStatement"></param>
        /// <returns></returns>
        public IEnumerable<IdentityEntity> FindIdentities(
            Func<IdentityEntity, bool> matchByStatement
        )
            => IdentityDbContext
                .Identities
                .Where(matchByStatement)
                .ToArray();
        #endregion

        #region FindProfiles
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="matchByStatement"></param>
        /// <returns></returns>
        public IEnumerable<ProfileEntity> FindProfiles(
            Func<ProfileEntity, bool> matchByStatement
        )
            => IdentityDbContext
                .Profiles
                .Include(m => m.Identity)
                .Where(matchByStatement)
                .ToArray();
        #endregion

        #endregion

        #region Mappers

        #region IdentityDtoToEntity
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="Identity"> </param>
        /// <returns></returns>
        private IdentityEntity IdentityDtoToEntity(
            IdentityDto identityDto,
            Guid identityId
        )
        {
            try
            {
                var identityEntity = new IdentityEntity
                {
                    Id = identityDto.Id,
                    Name = identityDto.Name,
                    CreatedBy = $"{identityId}",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = $"{identityId}",
                    ModifiedOn = DateTime.UtcNow
                };

                if(identityEntity == null)
                {
                    Logger.LogError($"");
                    SeriLogger.Error($"");
                    throw new Exception($"");
                }
                Logger.LogTrace($"");
                SeriLogger.Information($"");

                return identityEntity;
            }
            catch(Exception ex)
            {
                Logger.LogCritical(ex, $"");
                SeriLogger.Fatal(ex, $"");
                throw new Exception($"", ex);
            }
        }
        #endregion

        #region IdentityEntityToDto
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="msgEntity"> </param>
        /// <returns></returns>
        private IdentityDto IdentityEntityToDto(
            IdentityEntity identityEntity
        )
        {
            try
            {
                var identityDto = new IdentityDto
                {
                    Id = default,
                    Name = null,
                };

                Logger.LogTrace($"");
                SeriLogger.Information($"");

                return identityDto;
            }
            catch(Exception ex)
            {
                Logger.LogCritical(ex, $"");
                SeriLogger.Fatal(ex, $"");
                throw new Exception($"", ex);
            }
        }
        #endregion

        #region ProfileDtoToEntity
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="Profile"> </param>
        /// <returns></returns>
        private ProfileEntity ProfileDtoToEntity(
            ProfileDto msgDto
        )
        {
            try
            {
                var msgEntity = new ProfileEntity
                {
                    Id = msgDto.Id,
                    IdentityId = msgDto.IdentityId,
                    CreatedBy = $"{msgDto.IdentityId}",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = $"{msgDto.IdentityId}",
                    ModifiedOn = DateTime.UtcNow
                };

                if(msgEntity == null)
                {
                    Logger.LogError($"");
                    SeriLogger.Error($"");
                    throw new Exception($"");
                }
                Logger.LogTrace($"");
                SeriLogger.Information($"");

                return msgEntity;
            }
            catch(Exception ex)
            {
                Logger.LogCritical(ex, $"");
                SeriLogger.Fatal(ex, $"");
                throw new Exception($"", ex);
            }
        }
        #endregion

        #region ProfileEntityToDto
        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="msgEntity"> </param>
        /// <returns></returns>
        private ProfileDto ProfileEntityToDto(
            ProfileEntity profileEntity
        )
        {
            try
            {
                var profileDto = new ProfileDto
                {
                    Id = profileEntity.Id,
                    IdentityId = profileEntity.IdentityId,
                };

                Logger.LogTrace($"");
                SeriLogger.Information($"");

                return profileDto;
            }
            catch(Exception ex)
            {
                Logger.LogCritical(ex, $"");
                SeriLogger.Fatal(ex, $"");
                throw new Exception($"", ex);
            }
        }
        #endregion

        #endregion
    }
}