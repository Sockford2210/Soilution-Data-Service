using Soilution.DataService.DataRepository.Models;

namespace Soilution.DataService.DataRepository.Repositories
{
    public interface IDataHubRepository
    {
        /// <summary>
        /// Creates new data record for data device.
        /// </summary>
        Task<int> CreateDataDeviceRecord(DataHubRecord record);

        /// <summary>
        /// Gets a data device record specified by name.
        /// </summary>
        Task<DataHubRecord> GetDataDeviceRecordByName(string name);
    }
}
