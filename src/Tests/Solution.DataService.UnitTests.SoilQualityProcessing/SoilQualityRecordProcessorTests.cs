using Moq;
using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;
using Soilution.DataService.SoilQualityProcessing.Models;
using Soilution.DataService.SoilQualityProcessing.Services;

namespace Solution.DataService.UnitTests.SoilQualityProcessing
{
    public class SoildQualityRecordProcessorTests
    {
        private static SoilQualityProcessingService CreateSubjectUnderTest(Mock<ISoilDataRepository>? soilDataRepository = null)
        {
            soilDataRepository ??= new Mock<ISoilDataRepository>();

            return new SoilQualityProcessingService(soilDataRepository.Object);
        }

        [TestFixture]
        public class SoilQualityProcessingService_SubmitSoilQualityReading
        {
            [Test]
            public void DataRepositoryThrowsException_ThrowsException()
            {
                var soilDataRepository = new Mock<ISoilDataRepository>();
                var expectedException = new Exception("Expected Message");

                soilDataRepository.Setup(x => x.CreateNewSoilRecord(It.IsAny<SoilQualityDataRecord>()))
                    .Throws(expectedException);

                var sut = CreateSubjectUnderTest(soilDataRepository: soilDataRepository);

                var soilQuality = new SoilQualityReadingDto();
                var actualException = Assert.ThrowsAsync<Exception>(async () => await sut.SubmitSoilDataReading(soilQuality));
                Assert.That(actualException, Is.EqualTo(expectedException));
            }

            [Test]
            public async Task DataRepositoryDoesNotThrowException_DoesNothing()
            {
                var soilQuality = new SoilQualityReadingDto();
                var sut = CreateSubjectUnderTest();
                await sut.SubmitSoilDataReading(soilQuality);

                Assert.Pass();
            }
        }
    }
}