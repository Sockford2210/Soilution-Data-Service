using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;

namespace Soilution.DataService.MockedData.Repositories
{
    /// <summary>
    /// Mocked database repository for soil quality data.
    /// </summary>
    internal class SoilMockedRepository : ISoilDataRepository
    {
        /// <inheritdoc/>
        public Task<int> CreateNewSoilRecord(SoilQualityDataRecord record)
        {
            return Task.FromResult(1);
        }
    }
}
