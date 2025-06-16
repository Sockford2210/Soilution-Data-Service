namespace Soilution.DataService.AirQualityProcessing.Exceptions
{
    public class ParentDeviceDoesNotExistException : Exception
    {
        public ParentDeviceDoesNotExistException(string deviceName) :
            base($"The device with name: '{deviceName}' does not exist.")
        { }
    }
}