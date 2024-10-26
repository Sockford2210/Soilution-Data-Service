using Microsoft.AspNetCore.Mvc;
using Soilution.DataService.DeviceManagement.Devices.Exceptions;
using Soilution.DataService.DeviceManagement.Devices.Processors;

namespace Soilution.DataService.DeviceManagement.Devices.Controllers
{
    public class DataDeviceController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IDataDeviceProcessor _deviceProcessor;

        public DataDeviceController(ILogger logger, IDataDeviceProcessor deviceProcessor)
        {
            _logger = logger;
            _deviceProcessor = deviceProcessor ?? throw new ArgumentNullException(nameof(deviceProcessor));
        }

        public async Task<ActionResult> Post(string deviceName)
        {
            try
            {
                await _deviceProcessor.CreateNewDevice(deviceName);
            }
            catch (DeviceNameAlreadyTakenException)
            {
                var message = $"Device name: {deviceName} is aleady taken";
                return BadRequest(message);
            }

            return Ok();
        }
    }
}
