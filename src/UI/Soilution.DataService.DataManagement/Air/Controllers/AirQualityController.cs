using Microsoft.AspNetCore.Mvc;
using Soilution.DataService.DataManagement.Air.Models;
using Soilution.DataService.DataManagement.Air.Processors;

namespace Soilution.DataService.DataManagement.Controllers
{
    [ApiController]
    public class AirDataController : ControllerBase
    {
        private readonly ILogger<AirDataController> _logger;
        private readonly IAirQualityRecordProcessor _dataManager;

        public AirDataController(ILogger<AirDataController> logger, IAirQualityRecordProcessor dataManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
        }

        // POST api/AirData
        [HttpPost]
        [Route("api/[controller]")]
        public async Task<ActionResult> Post([FromBody] IncomingAirQualityReading incomingReading)
        {
            await _dataManager.SubmitAirQualityReading(incomingReading);

            _logger.LogInformation("New air quality reading submitted: {data}", incomingReading);

            return Ok();
        }

        // GET: api/AirData/Latest/{deviceName}/{count}
        [HttpGet]
        [Route("api/[controller]")]
        public async Task<ActionResult<IEnumerable<AirQuality>>> Latest(string deviceName, int count)
        {
            var readings = await _dataManager.GetLatestAirQualityReadings(deviceName, count);

            _logger.LogInformation("{count} readings retrieved.", count);

            return Ok(readings);
        }
    }
}
