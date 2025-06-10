namespace Soilution.DataService.DatabaseAccess.Accessor
{
    public interface IDatabaseAccessorFactory
    {
        IDatabaseAccessor GetDatabaseAccessor();
    }
}