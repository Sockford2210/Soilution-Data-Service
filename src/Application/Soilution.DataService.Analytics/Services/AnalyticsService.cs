using Soilution.DataService.Analytics.Models;
using Soilution.DataService.Analytics.Services.Interfaces;
using Soilution.DataService.DataRepository.Repositories;

namespace Soilution.DataService.Analytics.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IAirQualityDataRepository _airQualityRepository;

        public AnalyticsService(IAirQualityDataRepository airQualityRepository)
        {
            _airQualityRepository = airQualityRepository ?? throw new ArgumentNullException(nameof(airQualityRepository));
        }

        /// <inheritdoc/>
        public async Task<AirQualityStatistcs> GetAirQualityStatistics(DateTime? fromDateTime = null)
        {
            var airQualityMaxMinAverage = fromDateTime == null
                ? await _airQualityRepository.GetMinMaxAverageAirQualityData()
                : await _airQualityRepository.GetMinMaxAverageAirQualityDataSinceTimemstamp(fromDateTime.Value);

            var airQualityStatistics = new AirQualityStatistcs
            {
                MaximumHumidityPercentage = airQualityMaxMinAverage.MaximumHumidityPercentage,
                MinimumHumidityPercentage = airQualityMaxMinAverage.MaximumHumidityPercentage,
                AverageHumidityPercentage = airQualityMaxMinAverage.AverageHumidityPercentage,
                MaximumTemperatureCelcius = airQualityMaxMinAverage.MaximumTemperatureCelcius,
                MinimumTemperatureCelcius = airQualityMaxMinAverage.MinimumTemperatureCelcius,
                AverageTemperatureCelcius = airQualityMaxMinAverage.AverageTemperatureCelcius,
                MaximumCo2ppm = airQualityMaxMinAverage.MaximumCO2PPM,
                MinimumCo2ppm = airQualityMaxMinAverage.MinimumCO2PPM,
                AverageCo2ppm = airQualityMaxMinAverage.AverageCO2PPM
            };

            return airQualityStatistics;
        }
    }
}
