using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;

namespace Soilution.DataService.SqlRepository.Repositories
{
    internal class DataDeviceSqlRepository : IDataDeviceRepository
    {
        public Task<int> CreateDataDeviceRecord(DataDeviceRecord record)
        {
            throw new NotImplementedException();
        }

        public Task<DataDeviceRecord> GetDataDeviceRecordByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
