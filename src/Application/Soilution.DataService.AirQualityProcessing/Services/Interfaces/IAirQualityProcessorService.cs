using Soilution.DataService.AirQualityProcessing.Models;

namespace Soilution.DataService.AirQualityProcessing.Services
{
    public interface IAirQualityProcessorService
    {
        Task<IEnumerable<AirQualityReading>> GetLatestAirQualityReadings(string deviceName, int count);
        Task SubmitAirQualityReading(IncomingAirQualityReading airQuality);
    }
}