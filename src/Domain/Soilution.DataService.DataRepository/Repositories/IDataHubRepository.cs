using Soilution.DataService.DataRepository.Models;

namespace Soilution.DataService.DataRepository.Repositories
{
    public interface IDataHubRepository
    {
        Task<int> CreateDataDeviceRecord(DataHubRecord record);
        Task<DataHubRecord> GetDataDeviceRecordByName(string name);
    }
}
