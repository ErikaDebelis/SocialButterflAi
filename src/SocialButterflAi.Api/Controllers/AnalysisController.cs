using Microsoft.AspNetCore.Mvc;

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
            UploadDtoRequest request
        )
        {
            try
            {
                if (request.File == null
                    || request.File.Length == 0
                )
                {
                    Logger.LogError("No file uploaded");
                    return BadRequest("No file uploaded");
                }

                var extension = Path.GetExtension(request.File.FileName).ToLowerInvariant().TrimStart('.');
                var matchingExtension = Enum.TryParse<VideoFormat>(extension, out var format) ?? VideoFormat.unknown;

                if (matchingExtension == VideoFormat.unknown)
                {
                    Logger.LogError("Invalid video format");
                    return BadRequest("Invalid video format");
                }

                var uploadResponse = await AnalysisService.UploadAsync(request);

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