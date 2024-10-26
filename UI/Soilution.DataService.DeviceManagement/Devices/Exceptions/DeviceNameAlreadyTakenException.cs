namespace Soilution.DataService.DeviceManagement.Devices.Exceptions
{
    public class DeviceNameAlreadyTakenException : Exception
    {
        public DeviceNameAlreadyTakenException(string deviceName) 
            : base(deviceName)
        { } 
    }
}
