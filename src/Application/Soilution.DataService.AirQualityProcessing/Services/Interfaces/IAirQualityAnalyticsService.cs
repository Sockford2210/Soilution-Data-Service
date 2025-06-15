using Soilution.DataService.AirQualityProcessing.Models;

namespace Soilution.DataService.AirQualityProcessing.Services.Interfaces
{
    public interface IAirQualityAnalyticsService
    {
        Task<AirQualityStatistcsDto> GetAirQualityStatistics(DateTime? fromDateTime = null, DateTime? toDateTime = null);
    }
}