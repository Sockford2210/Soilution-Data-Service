using Microsoft.AspNetCore.Mvc;
using Soilution.DataService.DataManagement.Air.Exceptions;
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
            try
            {
                await _dataManager.SubmitAirQualityReading(incomingReading);

                _logger.LogInformation($"AirDataController: New air quality reading submitted: {incomingReading}");

                return Ok();
            }
            catch (DeviceDoesNotExistException ex)
            {
                var message = $"Device with name: {incomingReading.DeviceName} does not exist";
                return BadRequest(message);
            }
            catch (Exception ex)
            {
                var message = $"AirDataController: Exception occurred while adding new reading - {ex.Message}";
                _logger.LogError(ex, message);
                throw;
            }
        }

        // GET: api/AirData/Latest/{deviceName}/{count}
        [HttpGet]
        [Route("api/[controller]")]
        public async Task<ActionResult<IEnumerable<AirQuality>>> Latest(string deviceName, int count)
        {
            try
            {
                var readings = await _dataManager.GetLatestAirQualityReadings(deviceName, count);

                _logger.LogInformation($"AirDataController: {count} readings retrieved for device: {deviceName}.");

                return Ok(readings);
            }
            catch (DeviceDoesNotExistException ex)
            {
                var message = $"Device with name: {deviceName} does not exist";
                return BadRequest(message);
            }
            catch (Exception ex)
            {
                var message = $"AirDataController: Exception occurred while adding new reading - {ex.Message}";
                _logger.LogError(ex, message);
                throw;
            }
        }
    }
}
