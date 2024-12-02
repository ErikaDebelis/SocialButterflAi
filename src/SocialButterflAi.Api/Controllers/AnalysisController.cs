using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialButterflAi.Services.Analysis;
using SocialButterflAi.Models.Analysis;

namespace SocialButterflAi.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnalysisController : ControllerBase
    {
        public IAnalysisService AnalysisService;
        public ILogger<AnalysisController> Logger;

        public AnalysisController(
            IAnalysisService analysisService,
            ILogger<AnalysisController> logger
        )
        {
            AnalysisService = analysisService;
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
                                                            file,
                                                            videoFormat
                                                        );

                if (uploadResponse == null
                    || !uploadResponse.Success
                    || string.IsNullOrWhiteSpace(uploadResponse.VideoPath)
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
            AnalysisDtoRequest request
        )
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.VideoPath))
                {
                    Logger.LogError("No video path provided");
                    return BadRequest("No video path provided");
                }

                var serviceRequest = new AnalysisRequest
                {
                    Language = request.Language
                };

                var analysis = await AnalysisService.AnalyzeAsync(serviceRequest);

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