namespace Soilution.DataService.AirQualityProcessing.Models
{
    public class AirQualityReadingDto
    {
        public int Id { get; set; } = -1;
        public string DeviceName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.MinValue;
        public double HumidityPercentage { get; set; } = -1;
        public double TemperatureCelcius { get; set; } = -1;
        public double Co2ppm { get; set; } = -1;
    }
}
