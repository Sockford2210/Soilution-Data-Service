using Microsoft.AspNetCore.Mvc;
using Soilution.DataService.SoilQualityProcessing.Models;
using Soilution.DataService.SoilQualityProcessing.Services;

namespace Soilution.DataService.DataManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoilQualityController : ControllerBase
    {
        private readonly ISoilQualityProcessingService _dataProcessor;

        public SoilQualityController(ISoilQualityProcessingService dataProcessor)
        {
            _dataProcessor = dataProcessor ?? throw new ArgumentNullException(nameof(dataProcessor));
        }

        // POST api/SoilQuality
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IncomingSoilQualityReading incomingReading)
        {
            if (incomingReading.DeviceId == null) return BadRequest("Device Id cannot be null");

            await _dataProcessor.SubmitSoilDataReading(incomingReading);

            return Ok();
        }
    }
}