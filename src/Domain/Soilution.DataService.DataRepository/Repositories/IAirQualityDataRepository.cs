using Soilution.DataService.DataRepository.Models;

namespace Soilution.DataService.DataRepository.Repositories
{
    /// <summary>
    /// Database repository for air quality data.
    /// </summary>
    public interface IAirQualityDataRepository
    {
        /// <summary>
        /// Creates a new air quality data record in the database.
        /// </summary>
        /// <param name="record">New record to be created.</param>
        Task<int> CreateNewAirQualityRecord(AirQualityDataRecord record);

        /// <summary>
        /// Retrieves latest air quality data records from the database.
        /// </summary>
        /// <param name="count">Number of records to retrieve.</param>
        Task<IEnumerable<AirQualityDataRecord>> GetLatestAirQualityRecords(int deviceId, int count);

        /// <summary>
        /// Retrieves the maximum humidity, temperature and co2 concentration from the air quality records stored in the database.
        /// </summary>
        Task<AirQualityDataMaxMinAverage> GetMinMaxAverageAirQualityData();

        /// <summary>
        /// Retrieves the maximum humidity, temperature and co2 concentration from the air quality records stored in the database.
        /// Only takes into account data from a specific point.
        /// </summary>
        Task<AirQualityDataMaxMinAverage> GetMinMaxAverageAirQualityDataInTimeRange(DateTime fromTimestamp, DateTime toTimestamp);

        /// <summary>
        /// Retrieves the total number of air quality records submitted for by a specified device.
        /// </summary>
        Task<int> GetNumberOfRecordsForDevice(int deviceId);
    }
}