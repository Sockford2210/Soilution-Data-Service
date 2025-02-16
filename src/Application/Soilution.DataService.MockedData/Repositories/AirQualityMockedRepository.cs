using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;

namespace Soilution.DataService.MockedData.Repositories
{
    /// <summary>
    /// Mocked database repository for air quality data.
    /// </summary>
    internal class AirQualityMockedRepository : IAirQualityDataRepository
    {
        private readonly Random _random = new();

        /// <inheritdoc/>
        public Task<int> CreateNewAirQualityRecord(AirQualityDataRecord record)
        {
            return Task.FromResult(1);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<AirQualityDataRecord>> GetLatestAirQualityRecords(int count)
        {
            IEnumerable<AirQualityDataRecord> results = new List<AirQualityDataRecord>();

            for (int n = 1; n <= count; n++)
            {
                results = results.Append(new AirQualityDataRecord()
                {
                    Id = n,
                    DeviceId = 1000,
                    CO2PPM = RandomDoubleValue(1000, 9999),
                    HumidityPercentage = RandomDoubleValue(0, 100),
                    TemperatureCelcius = RandomDoubleValue(10, 40),
                    Timestamp = DateTime.Now.AddMinutes(n).AddSeconds(RandomDoubleValue(0, 60)),
                });
            };

            return Task.FromResult(results);
        }

        public Task<AirQualityDataMaxMinAverage> GetMinMaxAverageAirQualityData()
        {
            return Task.FromResult(new AirQualityDataMaxMinAverage()
            {
                MinimumCO2PPM = 800,
                MinimumHumidityPercentage = 10,
                MinimumTemperatureCelcius = 15,
                MaximumCO2PPM = 1000,
                MaximumHumidityPercentage = 50,
                MaximumTemperatureCelcius = 25,
            });
        }

        public Task<AirQualityDataMaxMinAverage> GetMinMaxAverageAirQualityDataSinceTimemstamp(DateTime fromTimestamp)
        {
            return Task.FromResult(new AirQualityDataMaxMinAverage()
            {
                MinimumCO2PPM = 800,
                MinimumHumidityPercentage = 10,
                MinimumTemperatureCelcius = 15,
                MaximumCO2PPM = 1000,
                MaximumHumidityPercentage = 50,
                MaximumTemperatureCelcius = 25,
            });
        }

        private double RandomDoubleValue(int minValue, int maxValue)
        {
            var wholeValue = _random.Next(minValue, maxValue);
            var decimalValue = _random.NextDouble();

            return wholeValue + decimalValue;
        }
    }
}
