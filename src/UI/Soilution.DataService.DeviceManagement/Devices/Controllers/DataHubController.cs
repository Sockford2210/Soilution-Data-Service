using Microsoft.AspNetCore.Mvc;
using Soilution.DataService.DeviceManagement.Devices.Exceptions;
using Soilution.DataService.DeviceManagement.Devices.Models;
using Soilution.DataService.DeviceManagement.Devices.Processors;

namespace Soilution.DataService.DeviceManagement.Devices.Controllers
{
    public class DataHubController : ControllerBase
    {
        private readonly ILogger<DataHubController> _logger;
        private readonly IDataDeviceProcessor _deviceProcessor;

        public DataHubController(ILogger<DataHubController> logger, 
            IDataDeviceProcessor deviceProcessor)
        {
            _logger = logger;
            _deviceProcessor = deviceProcessor ?? throw new ArgumentNullException(nameof(deviceProcessor));
        }

        [HttpPost]
        [Route("api/[controller]")]
        public async Task<ActionResult> AddNewDevice(NewDataHub dataHub)
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
                var message = $"DataHubController"
                _logger.LogError(ex,)
            }

            return Ok();
        }

        [HttpGet]
        [Route("api/[controller]")]
        public async Task<ActionResult> GetDeviceCreatedDate(string deviceName)
        {
            try
            {
                var deviceDetails = await _deviceProcessor.GetDeviceDetailsByDeviceName(deviceName);
                return Ok(deviceDetails.DateRegistered);
            }
            catch (DeviceDoesNotExistException)
            {
                var message = $"Device with name: {deviceName} does not exist";
                return BadRequest(message);
            }
        }
    }
}
