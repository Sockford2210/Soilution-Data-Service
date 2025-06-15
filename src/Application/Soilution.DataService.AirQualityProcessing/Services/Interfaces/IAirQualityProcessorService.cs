using Soilution.DataService.AirQualityProcessing.Models;

namespace Soilution.DataService.AirQualityProcessing.Services
{
    public interface IAirQualityProcessorService
    {
        Task<IEnumerable<AirQualityReadingDto>> GetLatestAirQualityReadings(string deviceName, int count);
        Task SubmitAirQualityReading(AirQualityReadingDto airQuality);
    }
}