using Soilution.DataService.DatabaseAccess.CommandExecution;
using Soilution.DataService.DataRepository.Models.Base;

namespace Soilution.DataService.DatabaseAccess.CommandRunner
{
    public interface ICommandRunner
    {
        Task ExecuteCommandWithNoReturnData(DatabaseCommand command);
        Task<TOutput?> ExecuteCommandWithSingularOutputValue<TOutput>(DatabaseCommand command);
        Task<IEnumerable<TOutput>> ExecuteQueryAndReturnDataEnumerator<TOutput>(DatabaseCommand command) where TOutput : QueryResultObject, new();
        Task<TOutput> ExecuteQueryAndReturnSingleRecord<TOutput>(DatabaseCommand command) where TOutput : QueryResultObject, new();
    }
}
