﻿namespace Soilution.DataService.DataManagement.Air.Exceptions
{
    public class DeviceDoesNotExistException : Exception
    {
        public DeviceDoesNotExistException(string deviceName) :
            base($"The device with name: '{deviceName}' does not exist.")
        { }
    }
}