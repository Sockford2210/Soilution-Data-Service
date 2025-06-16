using Microsoft.AspNetCore.Mvc;
using Soilution.DataService.DataManagementApi.Soil.Models;
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

            var soilQuality = new SoilQualityReadingDto
            {
                DeviceId = incomingReading.DeviceId,
                MoisturePercentage = incomingReading.MoisturePercentage,
                PHLevel = incomingReading.PHLevel,
                SunlightLumens = incomingReading.SunlightLumens,
                TemperatureCelcius = incomingReading.TemperatureCelcius,
                Timestamp = incomingReading.Timestamp,
            };
            await _dataProcessor.SubmitSoilDataReading(soilQuality);

            return Ok();
        }

        //public async Task<IActionResult> NewDevice([FromBody] IncomingSoilQualityDevice incomingDevice)
        //{
        //    return Ok();
        //}
    }
}