using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SocialButterflAi.Data.Identity;
using SocialButterflAi.Data.Identity.Entities;
using SocialButterflAi.Services.Profile;


namespace SocialButterflAi.Api.Controllers
{
    ///<summary>
    /// WIP/ UNIMPLEMENTED
    ///</summary>
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private ILogger<ProfileController> Logger;
        private IdentityDbContext IdentityDbContext;
        private IProfileService ProfileService;

        public ProfileController(
            IProfileService profileService,
            IdentityDbContext identityDbContext,
            ILogger<ProfileController> logger
        )
        {
            ProfileService = profileService;
            IdentityDbContext = identityDbContext;
            Logger = logger;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetProfile(
        )
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error");
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateProfile(
            object request
        )
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
    }
}