using Soilution.DataService.AirQualityProcessing.Models;

namespace Soilution.DataService.AirQualityProcessing.Services.Interfaces
{
    public interface IAirQualityAnalyticsService
    {
        Task<AirQualityStatistcs> GetAirQualityStatistics(DateTime? fromDateTime = null);
    }
}