using Microsoft.AspNetCore.Mvc;
using Soilution.DataService.Analytics.Models;
using Soilution.DataService.Analytics.Services.Interfaces;

namespace Soilution.DataService.AnalyticsApi.Controllers
{
    [Route("api/Analytics/AirQuality")]
    [ApiController]
    public class AirQualityAnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AirQualityAnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService ?? throw new ArgumentNullException(nameof(analyticsService));
        }

        // GET: api/Analytics/AirQuality
        [HttpGet("Details")]
        public async Task<ActionResult<AirQualityStatistcs>> Details()
        {
            var result = await _analyticsService.GetAirQualityStatistics();
            return Ok(result);
        }
    }
}
