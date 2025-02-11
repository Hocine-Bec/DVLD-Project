using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;
using System.Data.Common;
using DVLD_DataAccess.Core.Repositories;

namespace DVLD.Tests.DataAccess
{
    public class TestsRepoTests
    {
        private const int ValidTestId = 1;
        private const int ValidPersonId = 1;
        private const int ValidLicenseTypeId = 1;
        private const int ValidTestTypeId = 1;
        private const int ValidLocalLicenseAppId = 1;
        private const int ValidTestAppointmentId = 1;

        #region Positive Tests

        [Fact]
        public async Task GetByIdAsync_WhenTestExists_ReturnCorrectTest()
        {
            // Arrange
            var test = CreateTestTest();
            var context = CreateInMemoryDbContext(test);
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByIdAsync(ValidTestId);

            // Assert
            result.Should().BeEquivalentTo(test);
        }

        [Fact]
        public async Task GetLastTestByPersonAndTestTypeAndLicenseTypeAsync_WhenTestExists_ReturnCorrectTest()
        {
            // Arrange
            var test = CreateTestTest();
            var context = CreateInMemoryDbContext(test);

            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetLastTestByPersonAndTestTypeAndLicenseTypeAsync(ValidPersonId, ValidLicenseTypeId, ValidTestTypeId);

            // Assert
            result.Should().BeEquivalentTo(test);
        }

        [Fact]
        public async Task GetAllTestsAsync_WhenTestsExist_ReturnListOfTests()
        {
            // Arrange
            var test = CreateTestTest();
            var context = CreateInMemoryDbContext(test);
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllTestsAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(test);
        }

        [Fact]
        public async Task AddTestAsync_WhenTestIsValid_ReturnTestId()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);
            var test = CreateTestTest();

            // Act
            var result = await repo.AddTestAsync(test);

            // Assert
            result.Should().Be(ValidTestId);
        }

        [Fact]
        public async Task UpdateTestAsync_WhenTestIsValid_ReturnTrue()
        {
            // Arrange
            var test = CreateTestTest();
            var context = CreateInMemoryDbContext(test);
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act
            test.Notes = "Updated notes"; // Modify a field
            var result = await repo.UpdateTestAsync(test);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetPassedTestCountAsync_WhenTestsExist_ReturnCorrectCount()
        {
            // Arrange
            var test = CreateTestTest();
            var context = CreateInMemoryDbContext(test);
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetPassedTestCountAsync(ValidLocalLicenseAppId);

            // Assert
            result.Should().Be(1);
        }

        [Fact]
        public async Task DoesPassTestTypeAsync_WhenTestPassed_ReturnTrue()
        {
            // Arrange
            var test = CreateTestTest();
            var context = CreateInMemoryDbContext(test);
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act
            var result = await repo.DoesPassTestTypeAsync(ValidLocalLicenseAppId, ValidTestTypeId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DoesAttendTestTypeAsync_WhenTestAttended_ReturnTrue()
        {
            // Arrange
            var test = CreateTestTest();
            var context = CreateInMemoryDbContext(test);
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act
            var result = await repo.DoesAttendTestTypeAsync(ValidLocalLicenseAppId, ValidTestTypeId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task TotalTrialsPerTestAsync_WhenTestsExist_ReturnCorrectCount()
        {
            // Arrange
            var test = CreateTestTest();
            var context = CreateInMemoryDbContext(test);
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act
            var result = await repo.TotalTrialsPerTestAsync(ValidLocalLicenseAppId, ValidTestTypeId);

            // Assert
            result.Should().Be(1);
        }

        [Fact]
        public async Task IsThereAnActiveScheduledTestAsync_WhenActiveTestExists_ReturnTrue()
        {
            // Arrange
            var test = CreateTestTest();
            var context = CreateInMemoryDbContext(test);
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act
            var result = await repo.IsThereAnActiveScheduledTestAsync(ValidLocalLicenseAppId, ValidTestTypeId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetTestIdByAppointmentIdAsync_WhenTestExists_ReturnTestId()
        {
            // Arrange
            var test = CreateTestTest();
            var context = CreateInMemoryDbContext(test);
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetTestIdByAppointmentIdAsync(ValidTestAppointmentId);

            // Assert
            result.Should().Be(ValidTestId);
        }

        #endregion

        #region Negative Tests

        [Fact]
        public async Task GetByIdAsync_WhenTestDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByIdAsync(ValidTestId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WhenTestIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetByIdAsync(-1));
        }

        [Fact]
        public async Task GetLastTestByPersonAndTestTypeAndLicenseTypeAsync_WhenPersonIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetLastTestByPersonAndTestTypeAndLicenseTypeAsync(-1, ValidLicenseTypeId, ValidTestTypeId));
        }

        [Fact]
        public async Task GetLastTestByPersonAndTestTypeAndLicenseTypeAsync_WhenLicenseTypeIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetLastTestByPersonAndTestTypeAndLicenseTypeAsync(ValidPersonId, -1, ValidTestTypeId));
        }

        [Fact]
        public async Task GetLastTestByPersonAndTestTypeAndLicenseTypeAsync_WhenTestTypeIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetLastTestByPersonAndTestTypeAndLicenseTypeAsync(ValidPersonId, ValidLicenseTypeId, -1));
        }

        [Fact]
        public async Task AddTestAsync_WhenTestIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.AddTestAsync(null!));
        }

        [Fact]
        public async Task UpdateTestAsync_WhenTestIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.UpdateTestAsync(null!));
        }

        [Fact]
        public async Task GetPassedTestCountAsync_WhenLocalLicenseAppIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetPassedTestCountAsync(-1));
        }

        [Fact]
        public async Task DoesPassTestTypeAsync_WhenLocalLicenseAppIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.DoesPassTestTypeAsync(-1, ValidTestTypeId));
        }

        [Fact]
        public async Task DoesPassTestTypeAsync_WhenTestTypeIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.DoesPassTestTypeAsync(ValidLocalLicenseAppId, -1));
        }

        [Fact]
        public async Task DoesAttendTestTypeAsync_WhenLocalLicenseAppIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.DoesAttendTestTypeAsync(-1, ValidTestTypeId));
        }

        [Fact]
        public async Task DoesAttendTestTypeAsync_WhenTestTypeIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.DoesAttendTestTypeAsync(ValidLocalLicenseAppId, -1));
        }

        [Fact]
        public async Task TotalTrialsPerTestAsync_WhenLocalLicenseAppIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.TotalTrialsPerTestAsync(-1, ValidTestTypeId));
        }

        [Fact]
        public async Task TotalTrialsPerTestAsync_WhenTestTypeIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.TotalTrialsPerTestAsync(ValidLocalLicenseAppId, -1));
        }

        [Fact]
        public async Task IsThereAnActiveScheduledTestAsync_WhenLocalLicenseAppIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.IsThereAnActiveScheduledTestAsync(-1, ValidTestTypeId));
        }

        [Fact]
        public async Task IsThereAnActiveScheduledTestAsync_WhenTestTypeIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.IsThereAnActiveScheduledTestAsync(ValidLocalLicenseAppId, -1));
        }

        [Fact]
        public async Task GetTestIdByAppointmentIdAsync_WhenTestAppointmentIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetTestIdByAppointmentIdAsync(-1));
        }

        [Fact]
        public async Task GetTestIdByAppointmentIdAsync_WhenTestDoesNotExist_ReturnZero()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetTestIdByAppointmentIdAsync(ValidTestAppointmentId);

            // Assert
            result.Should().Be(0);
        }

        #endregion

        #region Special Cases

        [Fact]
        public async Task AddTestAsync_WhenDbOperationFails_ThrowException()
        {
            // Arrange
            var mockDbContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<Test>>();

            mockDbContext.Setup(x => x.Tests).Returns(mockDbSet.Object);
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database operation fails"));

            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(mockDbContext.Object, mockLogger.Object);
            var test = CreateTestTest();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await repo.AddTestAsync(test));
        }

        [Fact]
        public async Task UpdateTestAsync_WhenDbOperationFails_ThrowException()
        {
            // Arrange
            var test = CreateTestTest();
            var mockDbContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<Test>>();

            mockDbContext.Setup(x => x.Tests).Returns(mockDbSet.Object);
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database operation fails"));

            var mockLogger = new Mock<ILogger<TestsRepo>>();
            var repo = new TestsRepo(mockDbContext.Object, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await repo.UpdateTestAsync(test));
        }

        #endregion

        #region Private Helpers

        private AppDbContext CreateInMemoryDbContext(Test test = null!)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                  .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test
                  .Options;

            var context = new AppDbContext(options);

            if (test is not null)
            {
                context.Tests.Add(test);
                context.SaveChanges();
            }

            return context;
        }

        private Test CreateTestTest() => new Test
        {
            Id = ValidTestId,
            TestAppointmentId = ValidTestAppointmentId,
            TestResult = true,
            Notes = "Test notes",
            UserId = 1,
        };

        #endregion
    }
}