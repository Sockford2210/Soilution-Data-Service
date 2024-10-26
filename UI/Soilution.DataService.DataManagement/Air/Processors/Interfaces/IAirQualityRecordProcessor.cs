using Soilution.DataService.DataManagement.Air.Models;

namespace Soilution.DataService.DataManagement.Air.Processors
{
    public interface IAirQualityRecordProcessor
    {
        Task<IEnumerable<AirQuality>> GetLatestAirQualityReadings(int count);
        Task SubmitAirQualityReading(IncomingAirQualityReading airQuality);
    }
}