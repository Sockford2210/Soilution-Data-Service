using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;

namespace Soilution.DataService.MockedData.Repositories
{
    internal class DataDeviceMockedRepository : IDataDeviceRepository
    {
        public Task<int> CreateDataDeviceRecord(DataDeviceRecord record)
        {
            return Task.FromResult(1);
        }

        public Task<DataDeviceRecord> GetDataDeviceRecordByName(string name)
        {
            if (name == "DefaultDevice")
            {
                var deviceRecord = new DataDeviceRecord
                {
                    Id = 1,
                    Name = name
                };

                return Task.FromResult(deviceRecord);
            }
            else
            {
                return Task.FromResult(new DataDeviceRecord());
            }
        }
    }
}
