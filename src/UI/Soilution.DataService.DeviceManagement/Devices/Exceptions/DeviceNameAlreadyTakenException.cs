namespace Soilution.DataService.DeviceManagement.Devices.Exceptions
{
    public class DeviceNameAlreadyTakenException : Exception
    {
        public DeviceNameAlreadyTakenException(string deviceName) 
            : base($"The device name: '{deviceName}' is already taken.")
        { } 
    }
}
