using Soilution.DataService.DataManagement.Soil.Models;
using Soilution.DataService.DataManagement.Soil.Processors;
using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;

namespace Solution.DataService.UnitTests.DataManagement
{
    public class SoildQualityRecordProcessorTests
    {
        private static SoilQualityRecordProcessor CreateSubjectUnderTest(Mock<ISoilDataRepository>? soilDataRepository = null)
        {
            soilDataRepository ??= new Mock<ISoilDataRepository>();

            return new SoilQualityRecordProcessor(soilDataRepository.Object);
        }

        [TestFixture]
        public class SoilQualityRecordProcessor_SubmitSoilQualityReading
        {
            [Test]
            public void DataRepositoryThrowsException_ThrowsException()
            {
                var soilDataRepository = new Mock<ISoilDataRepository>();
                var expectedException = new Exception("Expected Message");

                soilDataRepository.Setup(x => x.CreateNewSoilRecord(It.IsAny<SoilQualityDataRecord>()))
                    .Throws(expectedException);

                var sut = CreateSubjectUnderTest(soilDataRepository: soilDataRepository);

                var soilQuality = new IncomingSoilQualityReading();
                var actualException = Assert.ThrowsAsync<Exception>(async () => await sut.SubmitSoilDataReading(soilQuality));
                Assert.That(actualException, Is.EqualTo(expectedException));
            }

            [Test]
            public async Task DataRepositoryDoesNotThrowException_DoesNothing()
            {
                var soilQuality = new IncomingSoilQualityReading();
                var sut = CreateSubjectUnderTest();
                await sut.SubmitSoilDataReading(soilQuality);

                Assert.Pass();
            }
        }
    }
}