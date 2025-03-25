using Microsoft.AspNetCore.Mvc;
using Soilution.DataService.DeviceManagement.Processors;
using Soilution.DataService.HubManagement.Exceptions;
using Soilution.DataService.HubManagement.Models;

namespace Soilution.DataService.DeviceManagement.Devices.Controllers
{
    public class DataHubController : ControllerBase
    {
        private readonly ILogger<DataHubController> _logger;
        private readonly IDataDeviceProcessorService _deviceProcessor;

        public DataHubController(ILogger<DataHubController> logger,
            IDataDeviceProcessorService deviceProcessor)
        {
            _logger = logger;
            _deviceProcessor = deviceProcessor ?? throw new ArgumentNullException(nameof(deviceProcessor));
        }

        [HttpPost]
        [Route("api/[controller]")]
        public async Task<ActionResult> AddNewDevice([FromBody] NewDataHub dataHub)
        {
            var deviceName = dataHub.DeviceName;
            try
            {
                await _deviceProcessor.CreateNewDevice(deviceName);
            }
            catch (DeviceNameAlreadyTakenException)
            {
                var message = $"Device name: {deviceName} is aleady taken";
                return BadRequest(message);
            }
            catch (Exception ex)
            {
                var message = $"DataHubController: Exception occurred while adding new device - {ex.Message}";
                _logger.LogError(ex, message);
                throw;
            }

            return Ok();
        }

        [HttpGet]
        [Route("api/[controller]/{deviceName}")]
        public async Task<ActionResult> GetDeviceDetails(string deviceName)
        {
            try
            {
                var deviceDetails = await _deviceProcessor.GetDeviceDetailsByDeviceName(deviceName);
                return Ok(deviceDetails);
            }
            catch (DeviceDoesNotExistException)
            {
                var message = $"Device with name: {deviceName} does not exist";
                return NotFound(message);
            }
            catch (Exception ex)
            {
                var message = $"DataHubController: Exception occurred while retrieving device details - {ex.Message}";
                _logger.LogError(ex, message);
                throw;
            }
        }
    }
}
