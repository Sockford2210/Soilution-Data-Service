using Microsoft.Extensions.Logging;
using Moq;
using Soilution.DataService.DatabaseAccess.Accessor;
using Soilution.DataService.DatabaseAccess.CommandExecution;
using System.Data;

namespace Soilution.DataService.UnitTests.DatabaseAccess
{
    public class DatabaseCommandRunnerTests
    {
        private static SqlDatabaseCommandRunner CreateSubjectUnderTest(Mock<IDatabaseAccessor> databaseAccessorMock = null)
        {
            var loggerMock = new Mock<ILogger<SqlDatabaseCommandRunner>>();

            databaseAccessorMock ??= CreateEmptyDatabaseAccessorMock();
            var accessorFactoryMock = CreateAccessorFactoryMock(databaseAccessorMock);

            return new SqlDatabaseCommandRunner(loggerMock.Object, accessorFactoryMock.Object);
        }

        private static Mock<IDatabaseAccessor> CreateEmptyDatabaseAccessorMock()
        {
            var accessorMock = new Mock<IDatabaseAccessor>();
            accessorMock.Setup(x => x.ExecuteScalarAsync(It.IsAny<DatabaseCommand>()))
                .ReturnsAsync(true);
            accessorMock.Setup(x => x.ExecuteReaderAsync(It.IsAny<DatabaseCommand>()))
                .ReturnsAsync(It.IsAny<IDataReader>);
            accessorMock.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<DatabaseCommand>()));

            return accessorMock;
        }

        private static Mock<IDatabaseAccessorFactory> CreateAccessorFactoryMock(Mock<IDatabaseAccessor> databaseAccessorMock)
        {
            var accessorFactoryMock = new Mock<IDatabaseAccessorFactory>();
            accessorFactoryMock.Setup(x => x.GetDatabaseAccessor())
                .Returns(databaseAccessorMock.Object);

            return accessorFactoryMock;
        }

        private static DatabaseCommand CreateExampleDatabaseCommand()
        {
            var queryString = "TEST SQL COMMAND";
            return new DatabaseCommand(queryString);
        }

        public class ExecuteCommandWithNoReturnDataTests
        {
            [Test]
            public async Task ExecutionCompletes_MethodCompletesNoErrors()
            {
                var databaseCommand = CreateExampleDatabaseCommand();
                var sut = CreateSubjectUnderTest();

                await sut.ExecuteCommandWithNoReturnData(databaseCommand);

                Assert.True(true);
            }

            [Test]
            public async Task ExecutionThrowsException_MethodReThrows()
            {
                var databaseCommand = CreateExampleDatabaseCommand();
                var expectedException = new Exception("Test message");
                var databaseAccessorMock = new Mock<IDatabaseAccessor>();
                databaseAccessorMock.Setup(x => x.ExecuteNonQueryAsync(databaseCommand))
                    .ThrowsAsync(expectedException);

                var sut = CreateSubjectUnderTest(databaseAccessorMock);

                Assert.ThrowsAsync<Exception>(() => sut.ExecuteCommandWithNoReturnData(databaseCommand));
            }
        }

        public class ExecuteCommandWithSingularOutputValueTests
        {
            [Test]
            public async Task ExecutionCompletes_MethodCompletesNoErrors()
            {
                var databaseCommand = CreateExampleDatabaseCommand();
                var sut = CreateSubjectUnderTest();

                await sut.ExecuteCommandWithSingularOutputValue<int>(databaseCommand);

                Assert.True(true);
            }

            [Test]
            public async Task ExecutionThrowsException_MethodReThrows()
            {
                var databaseCommand = CreateExampleDatabaseCommand();
                var expectedException = new Exception("Test message");
                var databaseAccessorMock = new Mock<IDatabaseAccessor>();
                databaseAccessorMock.Setup(x => x.ExecuteScalarAsync(databaseCommand))
                    .ThrowsAsync(expectedException);

                var sut = CreateSubjectUnderTest(databaseAccessorMock);

                Assert.ThrowsAsync<Exception>(() => sut.ExecuteCommandWithSingularOutputValue<int>(databaseCommand));
            }
        }

        public class ExecuteQueryAndReturnDataEnumeratorTests
        {
            private static Mock<IDataReader> CreateDataReaderMock(IEnumerable<TestQueryResultObject> expectedResult)
            {
                var dataReaderMock = new Mock<IDataReader>();
                var readSequence = dataReaderMock.SetupSequence(x => x.Read());
                var stringValueSequence = dataReaderMock.SetupSequence(x => x[nameof(TestQueryResultObject.StringValue)]);
                var intValueSequence = dataReaderMock.SetupSequence(x => x[nameof(TestQueryResultObject.IntValue)]);
                foreach (var expectedRecord in expectedResult)
                {
                    readSequence.Returns(true);
                    stringValueSequence.Returns(expectedRecord.StringValue);
                    intValueSequence.Returns(expectedRecord.IntValue);
                }

                readSequence.Returns(false);

                return dataReaderMock;
            }

            [Test]
            public void ExecutionThrowsException_MethodReThrows()
            {
                var databaseCommand = CreateExampleDatabaseCommand();
                var expectedException = new Exception("Test message");
                var databaseAccessorMock = new Mock<IDatabaseAccessor>();
                databaseAccessorMock.Setup(x => x.ExecuteReaderAsync(databaseCommand))
                    .ThrowsAsync(expectedException);

                var sut = CreateSubjectUnderTest(databaseAccessorMock);

                Assert.ThrowsAsync<Exception>(() => sut.ExecuteQueryAndReturnDataEnumerator<TestQueryResultObject>(databaseCommand));
            }

            [Test]
            public async Task DataReaderReturnsResults_ObjectsInCollectionPopulated()
            {
                IEnumerable<TestQueryResultObject>? expectedResult = new List<TestQueryResultObject>
                {
                    new() {
                        StringValue = "Test String One",
                        IntValue = 1
                    },
                    new() {
                        StringValue = "Test String Two",
                        IntValue = 2
                    }
                };

                var dataReaderMock = CreateDataReaderMock(expectedResult);

                var databaseCommand = CreateExampleDatabaseCommand();
                var databaseAccessorMock = new Mock<IDatabaseAccessor>();
                databaseAccessorMock.Setup(x => x.ExecuteReaderAsync(databaseCommand))
                    .ReturnsAsync(dataReaderMock.Object);

                var sut = CreateSubjectUnderTest(databaseAccessorMock);

                var result = await sut.ExecuteQueryAndReturnDataEnumerator<TestQueryResultObject>(databaseCommand);

                var numberOfValuesPopulated = result.Count(x => x.StringValueSet && x.IntValueSet);
                Assert.That(numberOfValuesPopulated, Is.EqualTo(expectedResult.Count()));
            }

            [Test]
            public async Task DataReaderReturnsResults_ObjectsInCollectionMatch()
            {
                IEnumerable<TestQueryResultObject>? expectedResult = new List<TestQueryResultObject>
                {
                    new() {
                        StringValue = "Test String One",
                        IntValue = 1
                    },
                    new() {
                        StringValue = "Test String Two",
                        IntValue = 2
                    }
                };

                var dataReaderMock = CreateDataReaderMock(expectedResult);

                IEnumerable<string> expectedStringValues = expectedResult.Select(x => x.StringValue);
                IEnumerable<int> expectedIntValues = expectedResult.Select(x => x.IntValue);

                var databaseCommand = CreateExampleDatabaseCommand();
                var databaseAccessorMock = new Mock<IDatabaseAccessor>();
                databaseAccessorMock.Setup(x => x.ExecuteReaderAsync(databaseCommand))
                    .ReturnsAsync(dataReaderMock.Object);

                var sut = CreateSubjectUnderTest(databaseAccessorMock);

                var result = await sut.ExecuteQueryAndReturnDataEnumerator<TestQueryResultObject>(databaseCommand);

                CollectionAssert.AreEqual(expectedStringValues, result.Select(x => x.StringValue));
                CollectionAssert.AreEqual(expectedIntValues, result.Select(x => x.IntValue));
            }
        }
    }
}