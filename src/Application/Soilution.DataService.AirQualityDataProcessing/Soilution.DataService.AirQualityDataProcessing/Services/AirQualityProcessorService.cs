using Soilution.DataService.AirQualityProcessing.Exceptions;
using Soilution.DataService.AirQualityProcessing.Models;
using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;

namespace Soilution.DataService.AirQualityProcessing.Services
{
    internal class AirQualityProcessorService : IAirQualityProcessorService
    {
        private readonly IAirQualityDataRepository _dataRepository;
        private readonly IDataHubRepository _deviceRepository;

        public AirQualityProcessorService(IAirQualityDataRepository dataRepository, 
            IDataHubRepository deviceRepository)
        {
            _dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));
            _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
        }

        public async Task SubmitAirQualityReading(IncomingAirQualityReading airQualityReading)
        {
            var dataDevice = await _deviceRepository.GetDataDeviceRecordByName(airQualityReading.DeviceName);

            var deviceId = dataDevice.Exists 
                ? dataDevice.Id
                : await CreateNewDataDevice(airQualityReading.DeviceName);

            var record = new AirQualityDataRecord
            {
                DeviceId = deviceId,
                Timestamp = airQualityReading.Timestamp,
                HumidityPercentage = airQualityReading.HumidityPercentage,
                TemperatureCelcius = airQualityReading.TemperatureCelcius,
                CO2PPM = airQualityReading.Co2ppm,
            };

            await _dataRepository.CreateNewAirQualityRecord(record);
        }

        public async Task<IEnumerable<AirQualityReading>> GetLatestAirQualityReadings(string deviceName, int count)
        {
            var device = await _deviceRepository.GetDataDeviceRecordByName(deviceName);

            if (!device.Exists)
            {
                throw new DeviceDoesNotExistException(deviceName);
            }

            var deviceId = device.Id;

            var readings = await _dataRepository.GetLatestAirQualityRecords(deviceId, count);

            var airQualityData = readings.Select(x => new AirQualityReading()
            {
                Id = x.Id,
                DeviceId = x.DeviceId,
                Timestamp = x.Timestamp,
                HumidityPercentage = x.HumidityPercentage,
                TemperatureCelcius = x.TemperatureCelcius,
                Co2ppm = x.CO2PPM,
            }).ToList();

            return airQualityData;
        }

        private async Task<int> CreateNewDataDevice(string deviceName)
        {
            var dataDevice = new DataHubRecord
            {
                DateCreated = DateTime.Now,
                Name = deviceName,
            };

            return await _deviceRepository.CreateDataDeviceRecord(dataDevice);
        }
    }
}
