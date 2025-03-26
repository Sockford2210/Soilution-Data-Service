using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Soilution.DataService.DataRepository.Models.Base;

namespace Soilution.DataService.SqlDatabase
{
    /// <summary>
    /// SQL Database accessor, exposed inherited properties for interfacing with the underlying database.
    /// </summary>
    public abstract class DatabaseAccessor
    {
        private readonly ILogger<DatabaseAccessor> _logger;
        private readonly DatabaseAccessorSettings _settings;

        public DatabaseAccessor(ILogger<DatabaseAccessor> logger, IOptions<DatabaseAccessorSettings> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Execute a single command that does not return and results.
        /// </summary>
        /// <param name="command">Command to execute.</param>
        protected async Task ExecuteCommandWithNoReturnData(DatabaseCommand command)
        {
            try
            {
                using var connection = new SqlConnection(_settings.ConnectionString);

                var sqlCommand = new SqlCommand(command.QueryString, connection);

                foreach (var parameter in command.Parameters)
                {
                    sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                sqlCommand.Connection.Open();
                await sqlCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DatabaseAccessor: Failed to execute query");
                throw;
            }
        }

        /// <summary>
        /// Execute a single command that returns an output value
        /// </summary>
        /// <typeparam name="TOutput">Scalar type for query result.</typeparam>
        /// <param name="command">Command to execute.</param>
        protected async Task<TOutput?> ExecuteCommandWithSingularOutputValue<TOutput>(DatabaseCommand command)
        {
            try
            {
                using var connection = new SqlConnection(_settings.ConnectionString);

                var sqlCommand = new SqlCommand(command.QueryString, connection);

                foreach (var parameter in command.Parameters)
                {
                    sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                sqlCommand.Connection.Open();

                var output = await sqlCommand.ExecuteScalarAsync();

                if (output is TOutput outputCast)
                {
                    return outputCast;
                }

                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DatabaseAccessor: Failed to execute query");
                throw;
            }
        }

        /// <summary>
        /// Execute a single command and maps the results onto a <see cref="DataRecordBase"/>
        /// </summary>
        /// <typeparam name="TOutput"><see cref="DataRecordBase"/> to map results onto.</typeparam>
        /// <param name="command">Command to execute.</param>
        protected async Task<IEnumerable<TOutput>> ExecuteQueryAndReturnData<TOutput>(DatabaseCommand command) where TOutput : DataRecordBase, new()
        {
            try
            {
                using var connection = new SqlConnection(_settings.ConnectionString);

                var sqlCommand = new SqlCommand(command.QueryString, connection);

                foreach (var parameter in command.Parameters)
                {
                    sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                sqlCommand.Connection.Open();

                var readings = new List<TOutput>();
                using var dataReader = await sqlCommand.ExecuteReaderAsync();
                while (dataReader.Read())
                {
                    var reading = new TOutput();
                    reading.PopulateFromDataReader(dataReader);
                    readings.Add(reading);
                }

                return readings.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DatabaseAccessor: Failed to execute query");
                throw;
            }
        }
    }
}
