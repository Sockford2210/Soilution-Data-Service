using Microsoft.AspNetCore.Mvc;
using Soilution.DataService.AirQualityProcessing.Exceptions;
using Soilution.DataService.AirQualityProcessing.Models;
using Soilution.DataService.DataManagementApi.Air.Models;
using Soilution.DataService.AirQualityProcessing.Services;
using Soilution.DataService.AirQualityProcessing.Services.Interfaces;

namespace Soilution.DataService.DataManagementApi.Controllers
{
    [ApiController]
    public class AirDataController : ControllerBase
    {
        private readonly ILogger<AirDataController> _logger;
        private readonly IAirQualityProcessorService _dataProcessor;
        private readonly IAirQualityDeviceService _deviceProcessor;

        public AirDataController(ILogger<AirDataController> logger, 
            IAirQualityProcessorService dataProcessor,
            IAirQualityDeviceService deviceProcessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataProcessor = dataProcessor ?? throw new ArgumentNullException(nameof(dataProcessor));
            _deviceProcessor = deviceProcessor ?? throw new ArgumentNullException(nameof(deviceProcessor));
        }

        // POST api/AirData
        [HttpPost]
        [Route("api/[controller]")]
        public async Task<ActionResult> Post([FromBody] IncomingAirQualityReading incomingReading)
        {
            try
            {
                var airQuality = new AirQualityReadingDto
                {
                    DeviceName = incomingReading.DeviceName,
                    HumidityPercentage = incomingReading.HumidityPercentage,
                    TemperatureCelcius = incomingReading.TemperatureCelcius,
                    Co2ppm = incomingReading.Co2ppm,
                    Timestamp = incomingReading.Timestamp,
                };

                await _dataProcessor.SubmitAirQualityReading(airQuality);

                _logger.LogInformation($"AirDataController: New air quality reading submitted: {incomingReading}");

                return Ok();
            }
            catch (ParentDeviceDoesNotExistException)
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
        [Route("api/[controller]/latest")]
        public async Task<ActionResult<IEnumerable<AirQualityReadingDto>>> Latest(string deviceName, int count)
        {
            try
            {
                var readings = await _dataProcessor.GetLatestAirQualityReadings(deviceName, count);

                _logger.LogInformation($"AirDataController: {count} readings retrieved for device: {deviceName}.");

                return Ok(readings);
            }
            catch (ParentDeviceDoesNotExistException)
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

        // POST api/AirData
        [HttpPost]
        [Route("api/[controller]/device")]
        public async Task<ActionResult> RegisterNewDevice([FromBody] IncomingAirQualityDevice incomingDevice)
        {
            try
            {
                var device = new AirQualityDeviceDto
                {
                    Name = incomingDevice.Name,
                    HubName = incomingDevice.HubName,
                };
                await _deviceProcessor.CreateNewDevice(device);

                _logger.LogInformation($"AirDataController: New air quality device created: {incomingDevice}");

                return Ok();
            }
            catch (ParentDeviceDoesNotExistException)
            {
                var message = $"Hub with name: {incomingDevice.HubName} does not exist";
                return BadRequest(message);
            }
            catch (Exception ex)
            {
                var message = $"AirDataController: Exception occurred while adding new device - {ex.Message}";
                _logger.LogError(ex, message);
                throw;
            }
        }
    }
}
