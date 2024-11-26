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
        public async Task<IActionResult> Analyze(
            AnalysisDtoRequest request
        )
        {
            try
            {
                var extension = Path.GetExtension(request.AudioFile.FileName).ToLowerInvariant().TrimStart('.');

                var matchingExtension = Enum.TryParse<AudioFormat>(extension out var audioFormat) ?? AudioFormat.unknown;

                if (matchingExtension == AudioFormat.unknown)
                {
                    Logger.LogError("Invalid audio format");
                    return BadRequest("Invalid audio format");
                }

                var base64Audio = Convert.ToBase64String(request.AudioFile.OpenReadStream().ReadAllBytes());

                var serviceRequest = new AnalysisRequest
                {
                    Base64Audio = base64Audio,
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