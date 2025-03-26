namespace Soilution.DataService.SoilQualityProcessing.Models
{
    public class SoilQuality
    {
        public int Id { get; set; }
        public string DeviceId { get; set; } = "Unknown";
        public DateTime Timestamp { get; set; }
        public double MoisturePercentage { get; set; }
        public double PHLevel { get; set; }
        public double TemperatureCelcius { get; set; }
        public double SunlightLumens { get; set; }
    }
}
