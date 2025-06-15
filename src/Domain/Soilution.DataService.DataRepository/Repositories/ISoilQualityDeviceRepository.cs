namespace Soilution.DataService.DataRepository.Repositories
{
    public interface ISoilQualityDeviceRepository
    {
        void CreateNewDevice();
        int GetRecordCountForDevice(int id);
    }
}
