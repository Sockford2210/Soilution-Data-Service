using Soilution.DataService.DataManagement.Air.Models;

namespace Soilution.DataService.DataManagement.Air.Processors
{
    public interface IAirQualityRecordProcessor
    {
        Task<IEnumerable<AirQuality>> GetLatestAirQualityReadings(string deviceName, int count);
        Task SubmitAirQualityReading(IncomingAirQualityReading airQuality);
    }
}