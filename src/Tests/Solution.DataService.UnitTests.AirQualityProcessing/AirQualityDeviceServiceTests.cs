using Soilution.DataService.AirQualityProcessing.Exceptions;
using Soilution.DataService.AirQualityProcessing.Models;
using Soilution.DataService.AirQualityProcessing.Services;
using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.DataService.UnitTests.AirQualityProcessing
{
    public class AirQualityDeviceServiceTests
    {
        #region Constants
        private const int TEST_DEVICE_ID = 99;
        private const int TEST_HUB_ID = 77;
        private const string TEST_HUB_NAME = "TestHub";
        #endregion

        #region Default Records
        private static DataHubRecord DefaultDataHubRecord => new()
        {
            Id = TEST_DEVICE_ID,
            DateCreated = DateTime.MinValue,
            Name = TEST_HUB_NAME
        };

        private static AirQualityDeviceRecord DefaultAirQualityDeviceRecord => new()
        {
            Id = TEST_DEVICE_ID,
            DateCreated = DateTime.MinValue,
            HubId = TEST_HUB_ID,
        };
        #endregion

        private static AirQualityDeviceService CreateSubjectUnderTest(Mock<IAirQualityDeviceRepository>? deviceRepositoryMock = null,
            Mock<IDataHubRepository>? hubRepositoryMock = null)
        {
            deviceRepositoryMock ??= new Mock<IAirQualityDeviceRepository>();
            hubRepositoryMock ??= new Mock<IDataHubRepository>();

            return new AirQualityDeviceService(deviceRepositoryMock.Object, hubRepositoryMock.Object);
        }

        [TestFixture]
        public class CreateNewDeviceTests
        {
            public async Task ReturnsExpectedValue()
            {
                var deviceRepositoryMock = new Mock<IAirQualityDeviceRepository>();
                deviceRepositoryMock.Setup(x => x.CreateNewDevice(DefaultAirQualityDeviceRecord))
                    .ReturnsAsync(TEST_DEVICE_ID);

                var hubRepositoryMock = new Mock<IDataHubRepository>();
                hubRepositoryMock.Setup(x => x.GetDataDeviceRecordByName(TEST_HUB_NAME))
                    .ReturnsAsync(DefaultDataHubRecord);

                var sut = CreateSubjectUnderTest(deviceRepositoryMock, hubRepositoryMock);

                var device = new AirQualityDeviceDto
                {
                    Name = "TestDevice",
                    HubName = TEST_HUB_NAME,
                };

                await sut.CreateNewDevice(device);

                Assert.True(true);
            }

            public void HubDoesNotExist_ThrowsDeviceDoesNotExistException()
            {
                var noneExistentHub = new DataHubRecord();
                var hubRepositoryMock = new Mock<IDataHubRepository>();
                hubRepositoryMock.Setup(x => x.GetDataDeviceRecordByName(TEST_HUB_NAME))
                    .ReturnsAsync(noneExistentHub);

                var sut = CreateSubjectUnderTest(hubRepositoryMock: hubRepositoryMock);

                var device = new AirQualityDeviceDto
                {
                    Name = "TestDevice",
                    HubName = TEST_HUB_NAME,
                };

                Assert.ThrowsAsync<DeviceDoesNotExistException>(() => sut.CreateNewDevice(device));
            }
        }
    }
}
