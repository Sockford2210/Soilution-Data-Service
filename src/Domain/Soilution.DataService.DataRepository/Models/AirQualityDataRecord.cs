using System.Data;
using Soilution.DataService.DataRepository.Models.Base;

namespace Soilution.DataService.DataRepository.Models
{
    public class AirQualityDataRecord : DataRecordBase
    {
        public int Id { get; set; } = -1;
        public int DeviceId { get; set; } = -1;
        public DateTime Timestamp { get; set; } = DateTime.MinValue;
        public double HumidityPercentage { get; set; } = 0;
        public double TemperatureCelcius { get; set; } = 0;
        public double CO2PPM { get; set; } = 0;

        public override void PopulateFromDataReader(IDataReader reader)
        {
            if (int.TryParse(reader[nameof(Id)]?.ToString(), out int id)) { Id = id; }
            if (int.TryParse(reader[nameof(DeviceId)]?.ToString(), out int deviceId)) { DeviceId = deviceId; }
            if (DateTime.TryParse(reader[nameof(Timestamp)]?.ToString(), out DateTime timestamp)) { Timestamp = timestamp; }
            if (double.TryParse(reader[nameof(HumidityPercentage)]?.ToString(), out double humidity)) { HumidityPercentage = humidity; }
            if (double.TryParse(reader[nameof(TemperatureCelcius)]?.ToString(), out double temperature)) { TemperatureCelcius = temperature; }
            if (double.TryParse(reader[nameof(CO2PPM)]?.ToString(), out double co2)) { CO2PPM = co2; }
        }
    }
}
