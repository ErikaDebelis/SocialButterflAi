using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SocialButterflAi.Data.Identity;
using SocialButterflAi.Data.Identity.Entities;
using SocialButterflAi.Services.Analysis;

using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Models.LLMIntegration;
using SocialButterflAi.Services.CueCoach;

namespace SocialButterflAi.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnalysisController : ControllerBase
    {
        private ILogger<AnalysisController> Logger;
        private IdentityDbContext IdentityDbContext;
        private IAnalysisService AnalysisService;
        private ICueCoachService CueCoachService;

        public AnalysisController(
            IAnalysisService analysisService,
            ICueCoachService cueCoachService,
            IdentityDbContext identityDbContext,
            ILogger<AnalysisController> logger
        )
        {
            AnalysisService = analysisService;
            CueCoachService = cueCoachService;
            IdentityDbContext = identityDbContext;
            Logger = logger;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> UploadVideo(
            IFormFile file,
            UploadDtoRequest request
        )
        {
            try
            {
                var identityName = HttpContext?.User?.Identity?.Name;

                var identity = IdentityDbContext.Identities.SingleOrDefault(i => i.Email == identityName);
                
                var matchingChat = CueCoachService.FindChats(x =>
                    x.Members.FirstOrDefault(m => m.Id == identity.Id) != null
                    && x.Id == Guid.Parse(request.ChatId)
                ).FirstOrDefault();
                
                if (file == null
                    || file.Length == 0
                )
                {
                    Logger.LogError("No file uploaded");
                    return BadRequest("No file uploaded");
                }

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant().TrimStart('.');
                var matchingExtension = Enum.TryParse<VideoFormat>(extension, true, out var videoFormat);
                if (!matchingExtension)
                    videoFormat = VideoFormat.unknown;

                if (videoFormat == VideoFormat.unknown)
                {
                    Logger.LogError("Invalid video format");
                    return BadRequest("Invalid video format");
                }

                var uploadResponse = await AnalysisService.UploadAsync(
                                                        identity.Id,
                                                        file,
                                                        videoFormat,
                                                        request.Title,
                                                        request.Description,
                                                        null
                                                    );

                if (uploadResponse is not { Success: true }
                    || string.IsNullOrWhiteSpace(uploadResponse.Data.VideoPath)
                )
                {
                    Logger.LogError("Upload was not successful");
                    return BadRequest("Upload was not successful");
                }

                Logger.LogInformation("Upload completed");

                return Ok(uploadResponse);
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
        [Route("Analyze")]
        public async Task<IActionResult> Analyze(
            VideoAnalysisRequest request
        )
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.VideoPath))
                {
                    Logger.LogError("No video path provided");
                    return BadRequest("No video path provided");
                }

                var modelProvider = Enum.Parse<ModelProvider>(request.ModelProvider);
                
                var analysis = await AnalysisService.AnalyzeAsync(request);

                if (analysis == null)
                {
                    Logger.LogError("Analysis failed");
                    return BadRequest("Analysis failed");
                }

                Logger.LogInformation("Analysis completed");
                return Ok(analysis);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
    }
}