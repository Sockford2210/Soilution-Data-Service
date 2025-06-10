using Microsoft.Data.SqlClient;
using Soilution.DataService.DatabaseAccess.CommandExecution;
using System.Data;

namespace Soilution.DataService.DatabaseAccess.Accessor
{
    public class SqlDatabaseAccessor : IDatabaseAccessor
    {
        private const string NOT_READY_TO_EXECUTE_ERROR_MESSAGE = "Cannot execute SQL command, connection is not open";

        private SqlConnection _connection;

        public SqlDatabaseAccessor(DatabaseAccessorSettings settings)
        {
            _connection = new SqlConnection(settings.ConnectionString);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public void OpenConnection()
        {
            _connection.Open();
        }

        public async Task<IDataReader> ExecuteReaderAsync(DatabaseCommand command)
        {
            if (!IsConnectionOpen())
            {
                throw new InvalidOperationException(NOT_READY_TO_EXECUTE_ERROR_MESSAGE);
            }

            var sqlCommand = BuildSqlCommand(command);
            return await sqlCommand.ExecuteReaderAsync();
        }

        public async Task ExecuteNonQueryAsync(DatabaseCommand command)
        {
            if (!IsConnectionOpen())
            {
                throw new InvalidOperationException(NOT_READY_TO_EXECUTE_ERROR_MESSAGE);
            }

            var sqlCommand = BuildSqlCommand(command);
            await sqlCommand.ExecuteNonQueryAsync();
        }

        public async Task<object?> ExecuteScalarAsync(DatabaseCommand command)
        {
            if (!IsConnectionOpen())
            {
                throw new InvalidOperationException(NOT_READY_TO_EXECUTE_ERROR_MESSAGE);
            }

            var sqlCommand = BuildSqlCommand(command);
            return await sqlCommand.ExecuteScalarAsync();
        }

        private bool IsConnectionOpen() => _connection.State == ConnectionState.Open;

        private SqlCommand BuildSqlCommand(DatabaseCommand command)
        {
            var sqlCommand = new SqlCommand(command.QueryString, _connection);

            foreach (var parameter in command.Parameters)
            {
                sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            return sqlCommand;
        }
    }
}
