using Moq;
using Soilution.DataService.DataRepository.Repositories;
using Soilution.DataService.DataManagement.Air.Processors;
using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataManagement.Air.Models;

namespace Soilution.DataService.UnitTests.DataManagement
{
    public class AirQualityRecordProcessorTests
    {
        private static AirQualityRecordProcessor CreateSubjectUnderTest(Mock<IAirQualityDataRepository>? airDataRepository = null, 
            Mock<IDataDeviceRepository>? dataDeviceRepository = null)
        {
            airDataRepository ??= new Mock<IAirQualityDataRepository>();
            dataDeviceRepository ??= new Mock<IDataDeviceRepository>();

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
                var actualException = Assert.ThrowsAsync<Exception>(async () => await sut.SubmitAirQualityReading(airQuality));
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
                var airDataRepository = new Mock<IAirQualityDataRepository>();
                var expectedException = new Exception("Expected Message");

                airDataRepository.Setup(x => x.GetLatestAirQualityRecords(It.IsAny<int>()))
                    .Throws(expectedException);

                var sut = CreateSubjectUnderTest(airDataRepository: airDataRepository);

                var actualException = Assert.ThrowsAsync<Exception>(async () => await sut.GetLatestAirQualityReadings(5));
                Assert.That(actualException, Is.EqualTo(expectedException));
            }

            [Test]
            public async Task DataRepositoryReturns5Results_Returns5Results()
            {
                var airDataRepository = new Mock<IAirQualityDataRepository>();
                var airQualityRecords = new List<AirQualityDataRecord>()
                {
                    new(),
                    new(),
                    new(),
                    new(),
                    new(),
                };
                int recordCount = airQualityRecords.Count;

                airDataRepository.Setup(x => x.GetLatestAirQualityRecords(recordCount))
                    .ReturnsAsync(airQualityRecords);

                var sut = CreateSubjectUnderTest(airDataRepository: airDataRepository);

                var results = await sut.GetLatestAirQualityReadings(recordCount);

                Assert.That(results.Count, Is.EqualTo(recordCount));
            }

            [Test]
            public async Task DataRepositoryReturnsResults_ReturnsMatchingResults()
            {
                var airDataRepository = new Mock<IAirQualityDataRepository>();
                int recordCount = 5;
                var airQualityRecords = new List<AirQualityDataRecord>()
                {
                    new(),
                    new(),
                    new(),
                    new(),
                    new(),
                };

                airDataRepository.Setup(x => x.GetLatestAirQualityRecords(recordCount))
                    .ReturnsAsync(airQualityRecords);

                var sut = CreateSubjectUnderTest(airDataRepository: airDataRepository);

                var results = await sut.GetLatestAirQualityReadings(recordCount);

                Assert.That(results.Count, Is.EqualTo(recordCount));
            }
        }
    }
}
