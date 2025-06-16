using Soilution.DataService.AirQualityProcessing.Exceptions;
using Soilution.DataService.AirQualityProcessing.Models;
using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;

namespace Soilution.DataService.AirQualityProcessing.Services
{
    internal class AirQualityProcessorService : IAirQualityProcessorService
    {
        private readonly IAirQualityDataRepository _dataRepository;
        private readonly IAirQualityDeviceRepository _deviceRepository;

        public AirQualityProcessorService(IAirQualityDataRepository dataRepository,
            IAirQualityDeviceRepository deviceRepository)
        {
            _dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));
            _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
        }

        public async Task SubmitAirQualityReading(AirQualityReadingDto airQualityReading)
        {
            var device = await _deviceRepository.GetDeviceByName(airQualityReading.DeviceName);

            var deviceId = device.Exists
                ? device.Id
                : throw new ParentDeviceDoesNotExistException(airQualityReading.DeviceName);

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

        public async Task<IEnumerable<AirQualityReadingDto>> GetLatestAirQualityReadings(string deviceName, int count)
        {
            var device = await _deviceRepository.GetDeviceByName(deviceName);

            var deviceId = device.Exists
                ? device.Id
                : throw new ParentDeviceDoesNotExistException(deviceName);

            var readings = await _dataRepository.GetLatestAirQualityRecords(deviceId, count);

            var airQualityData = readings.Select(x => new AirQualityReadingDto()
            {
                Id = x.Id,
                DeviceName = device.Name,
                Timestamp = x.Timestamp,
                HumidityPercentage = x.HumidityPercentage,
                TemperatureCelcius = x.TemperatureCelcius,
                Co2ppm = x.CO2PPM,
            }).ToList();

            return airQualityData;
        }
    }
}
