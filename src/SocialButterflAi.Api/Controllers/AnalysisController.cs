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