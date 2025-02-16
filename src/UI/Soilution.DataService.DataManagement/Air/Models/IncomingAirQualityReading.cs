namespace Soilution.DataService.DataManagement.Air.Models
{
    public class IncomingAirQualityReading
    {
        public string DeviceName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.MinValue;
        public double HumidityPercentage { get; set; } = 0;
        public double TemperatureCelcius { get; set; } = 0;
        public double Co2ppm { get; set; } = 0;
    }
}
