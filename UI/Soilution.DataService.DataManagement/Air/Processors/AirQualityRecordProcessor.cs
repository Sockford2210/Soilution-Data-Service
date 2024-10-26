using Soilution.DataService.DataManagement.Air.Models;
using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;

namespace Soilution.DataService.DataManagement.Air.Processors
{
    internal class AirQualityRecordProcessor : IAirQualityRecordProcessor
    {
        private readonly IAirQualityDataRepository _dataRepository;
        private readonly IDataDeviceRepository _deviceRepository;

        public AirQualityRecordProcessor(IAirQualityDataRepository dataRepository, IDataDeviceRepository deviceRepository)
        {
            _dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));
            _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
        }

        public async Task SubmitAirQualityReading(IncomingAirQualityReading airQualityReading)
        {
            var deviceId = _deviceRepository.GetDataDeviceRecordByName(airQualityReading.DeviceName)?.Id
                ?? await CreateNewDataDevice(airQualityReading.DeviceName);

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

        public async Task<IEnumerable<AirQuality>> GetLatestAirQualityReadings(int count)
        {
            var readings = await _dataRepository.GetLatestAirQualityRecords(count);

            var airQualityData = readings.Select(x => new AirQuality()
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
            var dataDevice = new DataDeviceRecord
            {
                Name = deviceName,
            };

            return await _deviceRepository.CreateDataDeviceRecord(dataDevice);
        }
    }
}
