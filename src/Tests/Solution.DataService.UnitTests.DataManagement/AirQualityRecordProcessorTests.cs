using Soilution.DataService.DataRepository.Repositories;
using Soilution.DataService.DataManagement.Air.Processors;
using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataManagement.Air.Models;

namespace Soilution.DataService.UnitTests.DataManagement
{
    public class AirQualityRecordProcessorTests
    {
        #region Constants
        private const int TEST_DEVICE_ID = 99;
        private const string TEST_DEVICE_NAME = "TestDevice";
        #endregion

        #region Default Records
        private static DataHubRecord DefaultDataHubRecord => new()
        {
            Id = TEST_DEVICE_ID,
            DateCreated = DateTime.MinValue,
            Name = TEST_DEVICE_NAME
        };
        
        private static List<AirQualityDataRecord> DefaultAirQualityRecords => new()
        {
            new AirQualityDataRecord
            {
                Id = 1,
                DeviceId = TEST_DEVICE_ID,
                CO2PPM = 1000,
                Timestamp = new DateTime(2025, 01, 01, 10, 20, 30),
                TemperatureCelcius = 25,
                HumidityPercentage = 20
            },
            new AirQualityDataRecord
            {
                Id = 2,
                DeviceId = TEST_DEVICE_ID,
            },
            new AirQualityDataRecord
            {
                Id = 3,
                DeviceId = TEST_DEVICE_ID,
            },
            new AirQualityDataRecord
            {
                Id = 4,
                DeviceId = TEST_DEVICE_ID,
            },
            new AirQualityDataRecord
            {
                Id = 5,
                DeviceId = TEST_DEVICE_ID,
            }
        };
        #endregion

        private static AirQualityRecordProcessor CreateSubjectUnderTest(Mock<IAirQualityDataRepository>? airDataRepository = null, 
            Mock<IDataHubRepository>? dataDeviceRepository = null)
        {
            airDataRepository ??= new Mock<IAirQualityDataRepository>();
            dataDeviceRepository ??= new Mock<IDataHubRepository>();

            return new AirQualityRecordProcessor(airDataRepository.Object, dataDeviceRepository.Object);
        }

        [TestFixture]
        public class AirQualityRecordProcessor_SubmitAirQualityReading
        {
            [Test]
            public void DataRepositoryThrowsException_ThrowsException()
            {
                var airDataRepository = new Mock<IAirQualityDataRepository>();
                var expectedException = new Exception("Expected Message");

                airDataRepository.Setup(x => x.CreateNewAirQualityRecord(It.IsAny<AirQualityDataRecord>()))
                    .Throws(expectedException);

                var sut = CreateSubjectUnderTest(airDataRepository: airDataRepository);

                var airQuality = new IncomingAirQualityReading();
                var actualException = Assert.ThrowsAsync<Exception>(async () 
                    => await sut.SubmitAirQualityReading(airQuality));
                Assert.That(actualException, Is.EqualTo(expectedException));
            }

            [Test]
            public async Task DataRepositoryDoesNotThrowException_DoesNothing()
            {
                var airQuality = new IncomingAirQualityReading();
                var sut = CreateSubjectUnderTest();

                await sut.SubmitAirQualityReading(airQuality);

                Assert.Pass();
            }
        }


        [TestFixture]
        public class AirQualityRecordProcessor_GetLatestAirQualityReadings
        {
            [Test]
            public void DataRepositoryThrowsException_ThrowsException()
            {
                var deviceRepository = new Mock<IDataHubRepository>();
                var airDataRepository = new Mock<IAirQualityDataRepository>();
                var expectedException = new Exception("Expected Message");
                
                deviceRepository.Setup(x => x.GetDataDeviceRecordByName(It.IsAny<string>()))
                    .ReturnsAsync(DefaultDataHubRecord);
                airDataRepository.Setup(x => x.GetLatestAirQualityRecords(It.IsAny<int>(), It.IsAny<int>()))
                    .Throws(expectedException);

                var sut = CreateSubjectUnderTest(airDataRepository, deviceRepository);

                var actualException = Assert.ThrowsAsync<Exception>(async () 
                    => await sut.GetLatestAirQualityReadings(TEST_DEVICE_NAME, 5));
                Assert.That(actualException, Is.EqualTo(expectedException));
            }

            [Test]
            public async Task DataRepositoryReturnsResults_ReturnsSameNumberOfResults()
            {
                var deviceRepository = new Mock<IDataHubRepository>();
                var airDataRepository = new Mock<IAirQualityDataRepository>();

                int expectedRecordCount = DefaultAirQualityRecords.Count;

                deviceRepository.Setup(x => x.GetDataDeviceRecordByName(It.IsAny<string>()))
                    .ReturnsAsync(DefaultDataHubRecord);
                airDataRepository.Setup(x => x.GetLatestAirQualityRecords(It.IsAny<int>(), expectedRecordCount))
                    .ReturnsAsync(DefaultAirQualityRecords);

                var sut = CreateSubjectUnderTest(airDataRepository, deviceRepository);

                var results = await sut.GetLatestAirQualityReadings(TEST_DEVICE_NAME, expectedRecordCount);

                Assert.That(results.Count, Is.EqualTo(expectedRecordCount));
            }

            [Test]
            public async Task DataRepositoryReturnsResult_ReturnsMatchingResults()
            {
                var deviceRepository = new Mock<IDataHubRepository>();
                var airDataRepository = new Mock<IAirQualityDataRepository>();
                var testAirQualityRecord = DefaultAirQualityRecords[0];
                var recordCount = 1;
                var expectedAirQuality = new AirQuality
                {
                    Id = testAirQualityRecord.Id,
                    DeviceId = testAirQualityRecord.DeviceId,
                    Timestamp = testAirQualityRecord.Timestamp,
                    Co2ppm = testAirQualityRecord.CO2PPM,
                    HumidityPercentage = testAirQualityRecord.HumidityPercentage,
                    TemperatureCelcius = testAirQualityRecord.TemperatureCelcius
                };

                deviceRepository.Setup(x => x.GetDataDeviceRecordByName(It.IsAny<string>()))
                    .ReturnsAsync(DefaultDataHubRecord);
                airDataRepository.Setup(x => x.GetLatestAirQualityRecords(testAirQualityRecord.DeviceId, recordCount))
                    .ReturnsAsync(new List<AirQualityDataRecord> { testAirQualityRecord });

                var sut = CreateSubjectUnderTest(airDataRepository, deviceRepository);

                var results = await sut.GetLatestAirQualityReadings(TEST_DEVICE_NAME, recordCount);
                var result = results.FirstOrDefault();

                Assert.Multiple(() =>
                {
                    Assert.That(result?.Id, Is.EqualTo(expectedAirQuality.Id));
                    Assert.That(result?.DeviceId, Is.EqualTo(expectedAirQuality.DeviceId));
                    Assert.That(result?.Timestamp, Is.EqualTo(expectedAirQuality.Timestamp));
                    Assert.That(result?.Co2ppm, Is.EqualTo(expectedAirQuality.Co2ppm));
                    Assert.That(result?.HumidityPercentage, Is.EqualTo(expectedAirQuality.HumidityPercentage));
                    Assert.That(result?.TemperatureCelcius, Is.EqualTo(expectedAirQuality.TemperatureCelcius));
                });
            }
        }
    }
}
