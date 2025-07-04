﻿using Soilution.DataService.DatabaseAccess.CommandExecution;
using Soilution.DataService.DatabaseAccess.CommandRunner;
using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;

namespace Soilution.DataService.DatabaseAccess.Repositories
{
    internal class AirQualityDeviceSqlRepository : IAirQualityDeviceRepository
    {
        private readonly ICommandRunner _commandRunner;

        private const string TABLE_NAME = "AirQualityDevice";
        private const string HUB_ID_PARAMETER = "@HubId";
        private const string DATE_CREATED_PARAMETER = "@DateCreated";
        private const string NAME_PARAMETER = "@Name";

        public AirQualityDeviceSqlRepository(ICommandRunner commandRunner)
        {
            _commandRunner = commandRunner ?? throw new ArgumentNullException(nameof(commandRunner));
        }

        public async Task<int> CreateNewDevice(AirQualityDeviceRecord newDevice)
        {
            string statement = $"INSERT INTO {TABLE_NAME} ({nameof(AirQualityDeviceRecord.HubId)}" +
                $" ,{nameof(AirQualityDeviceRecord.Name)}" +
                $" ,{nameof(AirQualityDeviceRecord.DateCreated)})" +
                $" OUTPUT INSERTED.Id" +
                $" VALUES ({HUB_ID_PARAMETER}" +
                $" ,{NAME_PARAMETER}" +
                $" ,{DATE_CREATED_PARAMETER})";

            var parameters = new Dictionary<string, object>
            {
                { HUB_ID_PARAMETER, newDevice.HubId },
                { NAME_PARAMETER, newDevice.Name },
                { DATE_CREATED_PARAMETER, newDevice.DateCreated }
            };

            var dBCommand = new DatabaseCommand(statement, parameters);

            return await _commandRunner.ExecuteCommandWithSingularOutputValue<int>(dBCommand);
        }

        public async Task<AirQualityDeviceRecord> GetDeviceByName(string deviceName)
        {
            var statement = $"SELECT TOP (1) [{nameof(AirQualityDeviceRecord.Id)}]" +
                $" ,[{nameof(AirQualityDeviceRecord.HubId)}]" +
                $" ,[{nameof(AirQualityDeviceRecord.DateCreated)}]" +
                $" FROM {TABLE_NAME}" +
                $" WHERE {nameof(AirQualityDeviceRecord.Name)} = {NAME_PARAMETER}";

            var parameters = new Dictionary<string, object>
            {
                { NAME_PARAMETER, deviceName }
            };

            var dBCommand = new DatabaseCommand(statement, parameters);

            return await _commandRunner.ExecuteQueryAndReturnSingleRecord<AirQualityDeviceRecord>(dBCommand);
        }
    }
}
