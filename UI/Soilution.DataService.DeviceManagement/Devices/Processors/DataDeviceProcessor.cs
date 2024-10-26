using Soilution.DataService.DeviceManagement.Devices.Exceptions;
using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;

namespace Soilution.DataService.DeviceManagement.Devices.Processors
{
    public class DataDeviceProcessor : IDataDeviceProcessor
    {
        private readonly IDataDeviceRepository _deviceRepository;

        public DataDeviceProcessor(IDataDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
        }

        public async Task CreateNewDevice(string deviceName)
        {
            var existingDeviceWithMatchingName = await _deviceRepository.GetDataDeviceRecordByName(deviceName);

            if (existingDeviceWithMatchingName.Exists)
            {
                throw new DeviceNameAlreadyTakenException(deviceName);
            }

            var newDeviceRecord = new DataDeviceRecord
            {
                Name = deviceName,
            };

            await _deviceRepository.CreateDataDeviceRecord(newDeviceRecord);
        }
    }
}
