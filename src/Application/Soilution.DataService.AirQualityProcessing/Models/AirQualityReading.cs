namespace Soilution.DataService.AirQualityProcessing.Models
{
    public class AirQualityReading
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public double HumidityPercentage { get; set; }
        public double TemperatureCelcius { get; set; }
        public double Co2ppm { get; set; }
    }
}
