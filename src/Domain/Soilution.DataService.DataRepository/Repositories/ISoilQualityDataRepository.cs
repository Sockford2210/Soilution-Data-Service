using Soilution.DataService.DataRepository.Models;

namespace Soilution.DataService.DataRepository.Repositories
{
    /// <summary>
    /// Database repository for soil quality data.
    /// </summary>
    public interface ISoilQualityDataRepository
    {
        /// <summary>
        /// Creates a new soil quality data record in the database.
        /// </summary>
        /// <param name="record">New record to be created.</param>
        Task<int> CreateNewSoilRecord(SoilQualityDataRecord record);
    }
}