using System.Data;
using Soilution.DataService.DataRepository.Models.Base;

namespace Soilution.DataService.DataRepository.Models
{
    public class SoilQualityDataRecord : DataRecordBase
    {
        public int Id { get; private set; }
        public string DeviceId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public double MoisturePercentage { get; set; }
        public double PHLevel { get; set; }
        public double TemperatureCelsius { get; set; }
        public double SunlightLumens { get; set; }

        public override void PopulateFromDataReader(IDataReader reader)
        {
            if (int.TryParse(reader[nameof(Id)]?.ToString(), out int id)) { Id = id; }
            DeviceId = reader[nameof(DeviceId)]?.ToString() ?? string.Empty;
            if (DateTime.TryParse(reader[nameof(Timestamp)]?.ToString(), out DateTime timestamp)) { Timestamp = timestamp; }
            if (double.TryParse(reader[nameof(MoisturePercentage)]?.ToString(), out double moisture)) { MoisturePercentage = moisture; }
            if (double.TryParse(reader[nameof(TemperatureCelsius)]?.ToString(), out double temperature)) { TemperatureCelsius = temperature; }
            if (double.TryParse(reader[nameof(PHLevel)]?.ToString(), out double ph)) { PHLevel = ph; }
            if (double.TryParse(reader[nameof(SunlightLumens)]?.ToString(), out double sunlight)) { SunlightLumens = sunlight; }
        }
    }
}
