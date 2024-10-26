using Soilution.DataService.DeviceManagement.Devices.Processors;
using Soilution.DataService.DataRepository.Repositories;
using Moq;
using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DeviceManagement.Devices.Exceptions;

namespace Soilution.DataService.UnitTests.DeviceManagement
{
    public class Tests
    {
        private static DataDeviceProcessor CreateSubjectUnderTest(Mock<IDataDeviceRepository>? dataDeviceRepository = null)
        {
            dataDeviceRepository ??= new Mock<IDataDeviceRepository>();

            return new DataDeviceProcessor(dataDeviceRepository.Object);
        }

        [TestFixture]
        public class DataDeviceProcessor_CreateNewDevice
        {
            [Test]
            public void DataRepositoryCreateDataDeviceRecordThrowsException_ThrowException()
            {
                var deviceRepository = new Mock<IDataDeviceRepository>();
                var expectedException = new Exception("Expected Message");
                var dataDeviceRecord = new DataDeviceRecord
                {
                    Id = -1,
                    Name = "Default Device"
                };

                deviceRepository.Setup(x => x.GetDataDeviceRecordByName(It.IsAny<string>()))
                    .ReturnsAsync(dataDeviceRecord);
                deviceRepository.Setup(x => x.CreateDataDeviceRecord(It.IsAny<DataDeviceRecord>()))
                    .ThrowsAsync(expectedException);

                var sut = CreateSubjectUnderTest(deviceRepository);

                var actualException = Assert.ThrowsAsync<Exception>(async () => await sut.CreateNewDevice(It.IsAny<string>()));
                Assert.That(actualException, Is.EqualTo(expectedException));
            }

            [Test]
            public void DataRepositoryGetDataDeviceRecordByNameThrowsException_ThrowException()
            {
                var deviceRepository = new Mock<IDataDeviceRepository>();
                var expectedException = new Exception("Expected Message");

                deviceRepository.Setup(x => x.GetDataDeviceRecordByName(It.IsAny<string>()))
                        .ThrowsAsync(expectedException);

                var sut = CreateSubjectUnderTest(deviceRepository);

                var actualException = Assert.ThrowsAsync<Exception>(async () => await sut.CreateNewDevice(It.IsAny<string>()));
                Assert.That(actualException, Is.EqualTo(expectedException));
            }

            [Test]
            public void DataRepositoryDataDeviceAlreadyExists_ThrowsDeviceNameAlreadyTakenException()
            {
                var deviceRepository = new Mock<IDataDeviceRepository>();
                var deviceName = "PrexistingDevice";
                var preExistingDeviceRecord = new DataDeviceRecord
                {
                    Id = 1,
                    Name = deviceName
                };

                deviceRepository.Setup(x => x.GetDataDeviceRecordByName(deviceName))
                    .ReturnsAsync(preExistingDeviceRecord);

                var sut = CreateSubjectUnderTest(deviceRepository);

                var exceptionThrown = Assert.ThrowsAsync<DeviceNameAlreadyTakenException>(async () => await sut.CreateNewDevice(deviceName));
            }
        }
    }
}