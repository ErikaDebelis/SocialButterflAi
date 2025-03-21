using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

using SocialButterflAi.Data.Chat;
using SocialButterflAi.Data.Identity;
using SocialButterflAi.Services.Analysis;
using SocialButterflAi.Data.Identity.Entities;
using SocialButterflAi.Data.Analysis.Entities;

using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Services.CueCoach;
using SocialButterflAi.Models.LLMIntegration;
using SocialButterflAi.Services.Helpers.Db.Queries;
using ChatEntity = SocialButterflAi.Data.Chat.Entities.Chat;
using MediaType = SocialButterflAi.Models.Analysis.MediaType;
using MessageEntity = SocialButterflAi.Data.Chat.Entities.Message;

namespace SocialButterflAi.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnalysisController : ControllerBase
    {
        private ILogger<AnalysisController> Logger;
        private IdentityDbContext IdentityDbContext;
        private ChatDbQueries ChatDbQueries;

        private IAnalysisService AnalysisService;

        public AnalysisController(
            IAnalysisService analysisService,
            IdentityDbContext identityDbContext,
            ChatDbContext chatDbContext,
            ILogger<AnalysisController> logger
        )
        {
            AnalysisService = analysisService;
            ChatDbQueries = new ChatDbQueries(chatDbContext, logger);
            IdentityDbContext = identityDbContext;
            Logger = logger;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAnalysis(
            string type,
            string path,
            string? id
        )
        {
            try
            {
                var identityName = HttpContext?.User?.Identity?.Name;

                var identity = IdentityDbContext.Identities.SingleOrDefault(i => i.Email == identityName);

                var parsedId = Guid.Parse(id);

                var analysisType = Enum.Parse<MediaType>(type);

                var matchingAnalysisResult = await AnalysisService.GetAnalysisAsync(
                                                                    identity.Id,
                                                                    analysisType,
                                                                    path,
                                                                    parsedId
                                                                );

                Logger.LogInformation("completed");

                return Ok(matchingAnalysisResult);
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

                var matchingChat = ChatDbQueries.FindEntities<ChatEntity>(x =>
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

                var uploadResponse = await AnalysisService.UploadVideoAsync(
                                                    identity.Id,
                                                    file,
                                                    videoFormat,
                                                    request.Title,
                                                    request.Description,
                                                    null
                                                );

                if (uploadResponse is not { Success: true }
                    || string.IsNullOrWhiteSpace(uploadResponse.Data.Path)
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