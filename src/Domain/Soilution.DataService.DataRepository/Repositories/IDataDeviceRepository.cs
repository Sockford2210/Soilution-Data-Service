using Soilution.DataService.DataRepository.Models;

namespace Soilution.DataService.DataRepository.Repositories
{
    public interface IDataDeviceRepository
    {
        Task<int> CreateDataDeviceRecord(DataDeviceRecord record);
        Task<DataDeviceRecord> GetDataDeviceRecordByName(string name);
    }
}
