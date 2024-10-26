using Microsoft.AspNetCore.Mvc;
using Soilution.DataService.DataManagement.Soil.Models;
using Soilution.DataService.DataManagement.Soil.Processors;

namespace Soilution.DataService.DataManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoilQualityController : ControllerBase
    {
        private readonly ISoilQualityRecordProcessor _dataManager;

        public SoilQualityController(ISoilQualityRecordProcessor dataManager)
        {
            _dataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
        }

        // POST api/SoilQuality
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IncomingSoilQualityReading incomingReading)
        {
            if (incomingReading.DeviceId == null) return BadRequest("Device Id cannot be null");

            await _dataManager.SubmitSoilDataReading(incomingReading);

            return Ok();
        }
    }
}