namespace Soilution.DataService.DeviceManagement.Devices.Exceptions
{
    public class DeviceDoesNotExistException : Exception
    {
        public DeviceDoesNotExistException(string deviceName) : 
            base($"The device with name: '{deviceName}' does not exist.") 
        { }
    }
}
