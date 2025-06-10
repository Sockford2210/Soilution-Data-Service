using Soilution.DataService.DatabaseAccess.CommandExecution;
using Soilution.DataService.DatabaseAccess.CommandRunner;
using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;

namespace Soilution.DataService.DatabaseAccess.Repositories
{
    internal class DataHubSqlRepository : IDataHubRepository
    {
        private readonly ICommandRunner _commandRunner;

        private const string DATA_HUB_TABLE_NAME = "DataHubs";
        private const string NAME_PARAMETER = "@Name";
        private const string DATE_CREATED_PARAMETER = "@DateCreated";

        public DataHubSqlRepository(ICommandRunner commandRunner)
        {
            _commandRunner = commandRunner ?? throw new ArgumentNullException(nameof(commandRunner));
        }

        /// <inheritdoc/>
        public async Task<int> CreateDataDeviceRecord(DataHubRecord record)
        {
            var insertStatement = $"INSERT INTO {DATA_HUB_TABLE_NAME} ({nameof(DataHubRecord.Name)}" +
                $" ,{nameof(DataHubRecord.DateCreated)})" +
                $" OUTPUT INSERTED.{nameof(DataHubRecord.Id)}" +
                $" VALUES ({NAME_PARAMETER}" +
                $" ,{DATE_CREATED_PARAMETER})";

            var parameters = new Dictionary<string, object>
            {
                { NAME_PARAMETER, record.Name },
                { DATE_CREATED_PARAMETER, record.DateCreated }
            };

            var dbCommand = new DatabaseCommand(insertStatement, parameters);

            return await _commandRunner.ExecuteCommandWithSingularOutputValue<int>(dbCommand);
        }

        /// <inheritdoc/>
        public async Task<DataHubRecord> GetDataDeviceRecordByName(string name)
        {
            var queryStatement = $"SELECT TOP (1) [{nameof(DataHubRecord.Id)}]" +
                $" ,[{nameof(DataHubRecord.Name)}]" +
                $" ,[{nameof(DataHubRecord.DateCreated)}]" +
                $" FROM {DATA_HUB_TABLE_NAME}" +
                $" WHERE {nameof(DataHubRecord.Name)} = {NAME_PARAMETER}";

            var parameters = new Dictionary<string, object>
            {
                { NAME_PARAMETER, name }
            };

            var dbCommand = new DatabaseCommand(queryStatement, parameters);

            return await _commandRunner.ExecuteQueryAndReturnSingleRecord<DataHubRecord>(dbCommand);
        }
    }
}
