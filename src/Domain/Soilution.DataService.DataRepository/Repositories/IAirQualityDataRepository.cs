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
    }
}