using Soilution.DataService.DataRepository.Repositories;
using Soilution.DataService.DataRepository.Models;
using NUnit.Framework.Internal;
using Microsoft.Extensions.Logging;
using Soilution.DataService.HubManagement.Processors;
using Soilution.DataService.HubManagement.Exceptions;

namespace Soilution.DataService.UnitTests.DeviceManagement
{
    public class DataDeviceRecordProcessorTests
    {
        private static DataDeviceProcessorService CreateSubjectUnderTest(Mock<IDataHubRepository>? dataDeviceRepository = null,
            Mock<IAirQualityDataRepository>? airDataRepository = null)
        {
            var logger = new Mock<ILogger<DataDeviceProcessorService>>();
            airDataRepository ??= new Mock<IAirQualityDataRepository>();
            dataDeviceRepository ??= new Mock<IDataHubRepository>();

            return new DataDeviceProcessorService(logger.Object,
                dataDeviceRepository.Object,
                airDataRepository.Object);
        }

        [TestFixture]
        public class DataDeviceProcessor_CreateNewDevice
        {
            [Test]
            public void DataRepositoryCreateDataDeviceRecordThrowsException_ThrowException()
            {
                var deviceRepository = new Mock<IDataHubRepository>();
                var expectedException = new Exception("Expected Message");
                var dataDeviceRecord = new DataHubRecord
                {
                    Id = -1,
                    Name = "Default Device"
                };

                deviceRepository.Setup(x => x.GetDataDeviceRecordByName(It.IsAny<string>()))
                    .ReturnsAsync(dataDeviceRecord);
                deviceRepository.Setup(x => x.CreateDataDeviceRecord(It.IsAny<DataHubRecord>()))
                    .ThrowsAsync(expectedException);

                var sut = CreateSubjectUnderTest(deviceRepository);

                var actualException = Assert.ThrowsAsync<Exception>(async () => await sut.CreateNewDevice(It.IsAny<string>()));
                Assert.That(actualException, Is.EqualTo(expectedException));
            }

            [Test]
            public void DataRepositoryGetDataDeviceRecordByNameThrowsException_ThrowException()
            {
                var deviceRepository = new Mock<IDataHubRepository>();
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
                var deviceRepository = new Mock<IDataHubRepository>();
                var deviceName = "PrexistingDevice";
                var preExistingDeviceRecord = new DataHubRecord
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