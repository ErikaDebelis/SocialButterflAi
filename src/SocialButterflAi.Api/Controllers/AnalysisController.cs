//RESTful API

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
            object request
        )
        {
            try
            {
                var analysis = await _analysisService.AnalyzeAsync(request);

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