using Microsoft.AspNetCore.Mvc;
using Soilution.DataService.AirQualityProcessing.Models;
using Soilution.DataService.AirQualityProcessing.Services.Interfaces;

namespace Soilution.DataService.DataAnalyticsApi.Controllers
{
    [Route("api/Analytics/AirQuality")]
    [ApiController]
    public class AirQualityAnalyticsController : ControllerBase
    {
        private readonly IAirQualityAnalyticsService _analyticsService;

        public AirQualityAnalyticsController(IAirQualityAnalyticsService analyticsService)
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
