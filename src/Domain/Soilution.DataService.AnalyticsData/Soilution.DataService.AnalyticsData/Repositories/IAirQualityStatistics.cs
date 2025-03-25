namespace Soilution.DataService.StatisticsData.Repositories
{
    public interface IAirQualityStatistics
    {
        /// <summary>
        /// Retrieves the maximum humidity, temperature and co2 concentration from the air quality records stored in the database.
        /// </summary>
        Task<AirQualityDataMaxMinAverage> GetMinMaxAverageAirQualityData();

        /// <summary>
        /// Retrieves the maximum humidity, temperature and co2 concentration from the air quality records stored in the database.
        /// Only takes into account data from a specific point.
        /// </summary>
        Task<AirQualityDataMaxMinAverage> GetMinMaxAverageAirQualityDataSinceTimemstamp(DateTime fromTimestamp);

        /// <summary>
        /// Retrieves the total number of air quality records submitted for by a specified device.
        /// </summary>
        Task<int> GetNumberOfRecordsForDevice(int deviceId);
    }
}
