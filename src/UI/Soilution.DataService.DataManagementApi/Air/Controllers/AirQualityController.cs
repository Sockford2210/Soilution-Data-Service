using Microsoft.AspNetCore.Mvc;
using Soilution.DataService.AirQualityProcessing.Exceptions;
using Soilution.DataService.AirQualityProcessing.Models;
using Soilution.DataService.AirQualityProcessing.Services;

namespace Soilution.DataService.DataManagementApi.Controllers
{
    [ApiController]
    public class AirDataController : ControllerBase
    {
        private readonly ILogger<AirDataController> _logger;
        private readonly IAirQualityProcessorService _dataProcessor;

        public AirDataController(ILogger<AirDataController> logger, IAirQualityProcessorService dataProcessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataProcessor = dataProcessor ?? throw new ArgumentNullException(nameof(dataProcessor));
        }

        // POST api/AirData
        [HttpPost]
        [Route("api/[controller]")]
        public async Task<ActionResult> Post([FromBody] IncomingAirQualityReading incomingReading)
        {
            try
            {
                await _dataProcessor.SubmitAirQualityReading(incomingReading);

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
        public async Task<ActionResult<IEnumerable<AirQualityReading>>> Latest(string deviceName, int count)
        {
            try
            {
                var readings = await _dataProcessor.GetLatestAirQualityReadings(deviceName, count);

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
