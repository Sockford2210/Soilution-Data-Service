using Soilution.DataService.Analytics.Models;

namespace Soilution.DataService.Analytics.Services.Interfaces
{
    public interface IAnalyticsService
    {
        /// <summary>
        /// Retrieves statistics on the currently stored air quality data.
        /// </summary>
        Task<AirQualityStatistcs> GetAirQualityStatistics(DateTime? fromDateTime = null);
    }
}