namespace Soilution.DataService.UnitTests.AirQualityProcessing
{
    public class AirQualityProcessorServiceTests
    {
        #region Constants
        private const int TEST_DEVICE_ID = 99;
        private const string TEST_DEVICE_NAME = "TestDevice";
        #endregion

        #region Default Records
        private static AirQualityDeviceRecord DefaultDataHubRecord => new()
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

        private static AirQualityProcessorService CreateSubjectUnderTest(Mock<IAirQualityDataRepository>? airDataRepository = null, 
            Mock<IAirQualityDeviceRepository>? dataDeviceRepository = null)
        {
            airDataRepository ??= new Mock<IAirQualityDataRepository>();
            dataDeviceRepository ??= new Mock<IAirQualityDeviceRepository>();

            return new AirQualityProcessorService(airDataRepository.Object, dataDeviceRepository.Object);
        }

        [TestFixture]
        public class AirQualityProcessorService_SubmitAirQualityReading
        {
            [Test]
            public void DataRepositoryThrowsException_ThrowsException()
            {
                var airDataRepository = new Mock<IAirQualityDataRepository>();
                var expectedException = new Exception("Expected Message");

                var airQuality = new AirQualityReadingDto()
                {
                    DeviceName = TEST_DEVICE_NAME,
                };

                airDataRepository.Setup(x => x.CreateNewAirQualityRecord(It.IsAny<AirQualityDataRecord>()))
                    .Throws(expectedException);

                var dataDeviceRepository = new Mock<IAirQualityDeviceRepository>();
                dataDeviceRepository.Setup(x => x.GetDeviceByName(TEST_DEVICE_NAME))
                    .ReturnsAsync(DefaultDataHubRecord);

                var sut = CreateSubjectUnderTest(airDataRepository: airDataRepository,
                    dataDeviceRepository: dataDeviceRepository);

                var actualException = Assert.ThrowsAsync<Exception>(async () 
                    => await sut.SubmitAirQualityReading(airQuality));
                Assert.That(actualException, Is.EqualTo(expectedException));
            }

            [Test]
            public async Task DataRepositoryDoesNotThrowException_DoesNothing()
            {
                var dataDeviceRepository = new Mock<IAirQualityDeviceRepository>();
                var airQuality = new AirQualityReadingDto()
                {
                    DeviceName = TEST_DEVICE_NAME,
                };

                dataDeviceRepository.Setup(x => x.GetDeviceByName(TEST_DEVICE_NAME))
                    .ReturnsAsync(DefaultDataHubRecord);

                var sut = CreateSubjectUnderTest(dataDeviceRepository: dataDeviceRepository);

                await sut.SubmitAirQualityReading(airQuality);

                Assert.Pass();
            }
        }

        [TestFixture]
        public class AirQualityProcessorService_GetLatestAirQualityReadings
        {
            [Test]
            public void DataRepositoryThrowsException_ThrowsException()
            {
                var deviceRepository = new Mock<IAirQualityDeviceRepository>();
                var airDataRepository = new Mock<IAirQualityDataRepository>();
                var expectedException = new Exception("Expected Message");
                
                deviceRepository.Setup(x => x.GetDeviceByName(TEST_DEVICE_NAME))
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
                var deviceRepository = new Mock<IAirQualityDeviceRepository>();
                var airDataRepository = new Mock<IAirQualityDataRepository>();

                int expectedRecordCount = DefaultAirQualityRecords.Count;

                deviceRepository.Setup(x => x.GetDeviceByName(TEST_DEVICE_NAME))
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
                var deviceRepository = new Mock<IAirQualityDeviceRepository>();
                var airDataRepository = new Mock<IAirQualityDataRepository>();
                var testAirQualityRecord = DefaultAirQualityRecords[0];
                var recordCount = 1;
                var expectedAirQuality = new AirQualityReadingDto
                {
                    Id = testAirQualityRecord.Id,
                    DeviceName = TEST_DEVICE_NAME,
                    Timestamp = testAirQualityRecord.Timestamp,
                    Co2ppm = testAirQualityRecord.CO2PPM,
                    HumidityPercentage = testAirQualityRecord.HumidityPercentage,
                    TemperatureCelcius = testAirQualityRecord.TemperatureCelcius
                };

                deviceRepository.Setup(x => x.GetDeviceByName(TEST_DEVICE_NAME))
                    .ReturnsAsync(DefaultDataHubRecord);
                airDataRepository.Setup(x => x.GetLatestAirQualityRecords(testAirQualityRecord.DeviceId, recordCount))
                    .ReturnsAsync(new List<AirQualityDataRecord> { testAirQualityRecord });

                var sut = CreateSubjectUnderTest(airDataRepository, deviceRepository);

                var results = await sut.GetLatestAirQualityReadings(TEST_DEVICE_NAME, recordCount);
                var result = results.FirstOrDefault();

                Assert.Multiple(() =>
                {
                    Assert.That(result?.Id, Is.EqualTo(expectedAirQuality.Id));
                    Assert.That(result?.DeviceName, Is.EqualTo(expectedAirQuality.DeviceName));
                    Assert.That(result?.Timestamp, Is.EqualTo(expectedAirQuality.Timestamp));
                    Assert.That(result?.Co2ppm, Is.EqualTo(expectedAirQuality.Co2ppm));
                    Assert.That(result?.HumidityPercentage, Is.EqualTo(expectedAirQuality.HumidityPercentage));
                    Assert.That(result?.TemperatureCelcius, Is.EqualTo(expectedAirQuality.TemperatureCelcius));
                });
            }
        }
    }
}
