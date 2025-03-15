using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;

namespace Soilution.DataService.MockedData.Repositories
{
    internal class DataHubMockedRepository : IDataHubRepository
    {
        public Task<int> CreateDataDeviceRecord(DataHubRecord record)
        {
            return Task.FromResult(1);
        }

        public Task<DataHubRecord> GetDataDeviceRecordByName(string name)
        {
            if (name == "DefaultDevice")
            {
                var deviceRecord = new DataHubRecord
                {
                    Id = 1,
                    Name = name
                };

                return Task.FromResult(deviceRecord);
            }
            else
            {
                return Task.FromResult(new DataHubRecord());
            }
        }
    }
}
