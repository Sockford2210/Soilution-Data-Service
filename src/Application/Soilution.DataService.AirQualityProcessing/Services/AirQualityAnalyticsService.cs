using Soilution.DataService.AirQualityProcessing.Services.Interfaces;
using Soilution.DataService.AirQualityProcessing.Models;
using Soilution.DataService.DataRepository.Repositories;

namespace Soilution.DataService.AirQualityProcessing.Services
{
    public class AirQualityAnalyticsService : IAirQualityAnalyticsService
    {
        private readonly IAirQualityDataRepository _airQualityRepository;

        public AirQualityAnalyticsService(IAirQualityDataRepository airQualityRepository)
        {
            _airQualityRepository = airQualityRepository ?? throw new ArgumentNullException(nameof(airQualityRepository));
        }

        /// <inheritdoc/>
        public async Task<AirQualityStatistcsDto> GetAirQualityStatistics(DateTime? fromDateTime = null, DateTime? toDateTime = null)
        {
            toDateTime ??= DateTime.MinValue;
            var airQualityMaxMinAverage = fromDateTime == null
                ? await _airQualityRepository.GetMinMaxAverageAirQualityData()
                : await _airQualityRepository.GetMinMaxAverageAirQualityDataInTimeRange(fromDateTime.Value, toDateTime.Value);

            var airQualityStatistics = new AirQualityStatistcsDto
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
