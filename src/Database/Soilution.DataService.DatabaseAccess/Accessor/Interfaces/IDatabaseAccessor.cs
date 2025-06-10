using Soilution.DataService.DatabaseAccess.CommandExecution;
using System.Data;

namespace Soilution.DataService.DatabaseAccess.Accessor
{
    public interface IDatabaseAccessor : IDisposable
    {
        void OpenConnection();
        Task<IDataReader> ExecuteReaderAsync(DatabaseCommand command);
        Task ExecuteNonQueryAsync(DatabaseCommand command);
        Task<object?> ExecuteScalarAsync(DatabaseCommand command);
    }
}
