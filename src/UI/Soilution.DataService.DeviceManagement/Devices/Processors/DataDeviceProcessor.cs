using Soilution.DataService.DeviceManagement.Devices.Exceptions;
using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;
using Soilution.DataService.DeviceManagement.Devices.Models;

namespace Soilution.DataService.DeviceManagement.Devices.Processors
{
    public class DataDeviceProcessor : IDataDeviceProcessor
    {
        private readonly ILogger<DataDeviceProcessor> _logger;
        private readonly IDataHubRepository _deviceRepository;
        private readonly IAirQualityDataRepository _airQualityDataRepository;

        public DataDeviceProcessor(ILogger<DataDeviceProcessor> logger,
            IDataHubRepository deviceRepository, 
            IAirQualityDataRepository airQualityDataRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
            _airQualityDataRepository = airQualityDataRepository ?? throw new ArgumentNullException(nameof(airQualityDataRepository));
        }

        public async Task CreateNewDevice(string deviceName)
        {
            var existingDeviceWithMatchingName = await _deviceRepository.GetDataDeviceRecordByName(deviceName);

            if (existingDeviceWithMatchingName.Exists)
            {
                var message = $"DataDeviceProcessor: Data device name: {deviceName} is already taken";
                _logger.LogWarning(message);
                throw new DeviceNameAlreadyTakenException(deviceName);
            }

            var newDeviceRecord = new DataHubRecord
            {
                Name = deviceName,
                DateCreated = DateTime.UtcNow,
            };

            await _deviceRepository.CreateDataDeviceRecord(newDeviceRecord);
        }

        public async Task<DataHubDetails> GetDeviceDetailsByDeviceName(string deviceName)
        {
            var deviceRecord = await _deviceRepository.GetDataDeviceRecordByName(deviceName);

            if (!deviceRecord.Exists)
            {
                var message = $"DataDeviceProcessor: Data device with name: {deviceName} does not exist";
                _logger.LogWarning(message);
                throw new DeviceDoesNotExistException(deviceName);
            }

            var numberOfRecords = await _airQualityDataRepository.GetNumberOfRecordsForDevice(deviceRecord.Id);

            var deviceDetails = new DataHubDetails
            {
                DeviceName = deviceRecord.Name,
                NumberOfRecords = numberOfRecords,
                DateRegistered = deviceRecord.DateCreated
            };

            return deviceDetails;
        }
    }
}
