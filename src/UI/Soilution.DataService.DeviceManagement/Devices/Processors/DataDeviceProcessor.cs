using Soilution.DataService.DeviceManagement.Devices.Exceptions;
using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;
using Soilution.DataService.DeviceManagement.Devices.Models;

namespace Soilution.DataService.DeviceManagement.Devices.Processors
{
    public class DataDeviceProcessor : IDataDeviceProcessor
    {
        private readonly IDataHubRepository _deviceRepository;
        private readonly IAirQualityDataRepository _airQualityDataRepository;

        public DataDeviceProcessor(IDataHubRepository deviceRepository)
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

            var newDeviceRecord = new DataHubRecord
            {
                Name = deviceName,
            };

            await _deviceRepository.CreateDataDeviceRecord(newDeviceRecord);
        }

        public async Task<DataHubDetails> GetDeviceDetailsByDeviceName(string deviceName)
        {
            var deviceRecord = await _deviceRepository.GetDataDeviceRecordByName(deviceName);

            if (!deviceRecord.Exists)
            {
                throw new DeviceDoesNotExistException(deviceName);
            }

            var numberOfRecords = await _airQualityDataRepository.GetNumberOfRecordsForDevice(deviceRecord.Id);

            var deviceDetails = new DataHubDetails
            {
                DeviceName = deviceRecord.Name,
                NumberOfRecords = numberOfRecords
            };

            return deviceDetails;
        }
    }
}
