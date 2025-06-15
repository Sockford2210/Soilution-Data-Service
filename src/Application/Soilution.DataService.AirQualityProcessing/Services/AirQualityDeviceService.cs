using Soilution.DataService.AirQualityProcessing.Exceptions;
using Soilution.DataService.AirQualityProcessing.Models;
using Soilution.DataService.AirQualityProcessing.Services.Interfaces;
using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;

namespace Soilution.DataService.AirQualityProcessing.Services
{
    public class AirQualityDeviceService : IAirQualityDeviceService
    {
        private readonly IAirQualityDeviceRepository _deviceRepository;
        private readonly IDataHubRepository _hubRepository;

        public AirQualityDeviceService(IAirQualityDeviceRepository deviceRepository, IDataHubRepository hubRepository)
        {
            _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
            _hubRepository = hubRepository ?? throw new ArgumentNullException(nameof(hubRepository));
        }

        public async Task CreateNewDevice(AirQualityDeviceDto newDevice)
        {
            var hubRecord = await _hubRepository.GetDataDeviceRecordByName(newDevice.HubName);

            if (!hubRecord.Exists) throw new DeviceDoesNotExistException(newDevice.HubName);

            var record = new AirQualityDeviceRecord
            {
                HubId = hubRecord.Id,
                DateCreated = DateTime.UtcNow,
            };

            await _deviceRepository.CreateNewDevice(record);
        }
    }
}
