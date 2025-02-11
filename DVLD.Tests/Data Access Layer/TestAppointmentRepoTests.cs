using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Interfaces;
using DVLD_DataAccess.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;

namespace DVLD.Tests.DataAccess
{
    public class TestAppointmentRepoTests
    {
        private const int ValidTestAppointmentId = 5;
        private const int ValidLocalLicenseAppId = 10;
        private const int ValidTestTypeId = 1;

        #region Positive Tests

        [Fact]
        public async Task GetByIdAsync_WhenTestAppointmentExists_ReturnCorrectTestAppointment()
        {
            // Arrange
            var testAppointment = CreateTestTestAppointment();
            var context = CreateInMemoryDbContext(testAppointment);

            var mockLogger = new Mock<ILogger<TestAppointmentRepo>>();
            var repo = new TestAppointmentRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByIdAsync(ValidTestAppointmentId);

            // Assert
            result.Should().BeEquivalentTo(testAppointment);
        }

        [Fact]
        public async Task GetLastTestAppointmentAsync_WhenTestAppointmentExists_ReturnCorrectTestAppointment()
        {
            // Arrange
            var testAppointment = CreateTestTestAppointment();
            var context = CreateInMemoryDbContext(testAppointment);

            var mockLogger = new Mock<ILogger<TestAppointmentRepo>>();
            var repo = new TestAppointmentRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetLastTestAppointmentAsync(ValidLocalLicenseAppId, ValidTestTypeId);

            // Assert
            result.Should().BeEquivalentTo(testAppointment);
        }

        [Fact]
        public async Task GetAllTestAppointmentsAsync_WhenTestAppointmentsExist_ReturnListOfTestAppointments()
        {
            // Arrange
            var testAppointment = CreateTestTestAppointment();
            var context = CreateInMemoryDbContext(testAppointment);

            var mockLogger = new Mock<ILogger<TestAppointmentRepo>>();
            var repo = new TestAppointmentRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllTestAppointmentsAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(testAppointment);
        }

        [Fact]
        public async Task GetAppTestAppointmentsPerTestTypeAsync_WhenTestAppointmentsExist_ReturnListOfTestAppointments()
        {
            // Arrange
            var testAppointment = CreateTestTestAppointment();
            var context = CreateInMemoryDbContext(testAppointment);

            var mockLogger = new Mock<ILogger<TestAppointmentRepo>>();
            var repo = new TestAppointmentRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAppTestAppointmentsPerTestTypeAsync(ValidLocalLicenseAppId, ValidTestTypeId);

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(testAppointment);
        }

        [Fact]
        public async Task AddTestAppointmentAsync_WhenTestAppointmentIsValid_ReturnTestAppointmentId()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestAppointmentRepo>>();
            var repo = new TestAppointmentRepo(context, mockLogger.Object);

            var testAppointment = CreateTestTestAppointment();

            // Act
            var result = await repo.AddTestAppointmentAsync(testAppointment);

            // Assert
            result.Should().Be(ValidTestAppointmentId);
        }

        [Fact]
        public async Task UpdateTestAppointmentAsync_WhenTestAppointmentIsValid_ReturnTrue()
        {
            // Arrange
            var testAppointment = CreateTestTestAppointment();
            var context = CreateInMemoryDbContext(testAppointment);

            var mockLogger = new Mock<ILogger<TestAppointmentRepo>>();
            var repo = new TestAppointmentRepo(context, mockLogger.Object);

            // Act
            testAppointment.AppointmentDate = DateTime.UtcNow.AddDays(1);
            var result = await repo.UpdateTestAppointmentAsync(testAppointment);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region Negative Tests

        [Fact]
        public async Task GetByIdAsync_WhenTestAppointmentDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestAppointmentRepo>>();
            var repo = new TestAppointmentRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByIdAsync(1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WhenTestAppointmentIdIsZeroOrMinus_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestAppointmentRepo>>();
            var repo = new TestAppointmentRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetByIdAsync(-1));
        }

        [Fact]
        public async Task GetLastTestAppointmentAsync_WhenTestAppointmentDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestAppointmentRepo>>();
            var repo = new TestAppointmentRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetLastTestAppointmentAsync(1, 1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetLastTestAppointmentAsync_WhenLocalLicenseAppIdIsZeroOrMinus_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestAppointmentRepo>>();
            var repo = new TestAppointmentRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetLastTestAppointmentAsync(-1, ValidTestTypeId));
        }

        [Fact]
        public async Task GetLastTestAppointmentAsync_WhenTestTypeIdIsZeroOrMinus_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestAppointmentRepo>>();
            var repo = new TestAppointmentRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetLastTestAppointmentAsync(ValidLocalLicenseAppId, -1));
        }

        [Fact]
        public async Task GetAllTestAppointmentsAsync_WhenNoTestAppointmentsExist_ReturnEmptyList()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestAppointmentRepo>>();
            var repo = new TestAppointmentRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllTestAppointmentsAsync();

            // Assert
            result.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetAppTestAppointmentsPerTestTypeAsync_WhenNoTestAppointmentsExist_ReturnEmptyList()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestAppointmentRepo>>();
            var repo = new TestAppointmentRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAppTestAppointmentsPerTestTypeAsync(1, 1);

            // Assert
            result.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetAppTestAppointmentsPerTestTypeAsync_WhenLocalLicenseAppIdIsZeroOrMinus_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestAppointmentRepo>>();
            var repo = new TestAppointmentRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetAppTestAppointmentsPerTestTypeAsync(-1, ValidTestTypeId));
        }

        [Fact]
        public async Task GetAppTestAppointmentsPerTestTypeAsync_WhenTestTypeIdIsZeroOrMinus_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestAppointmentRepo>>();
            var repo = new TestAppointmentRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetAppTestAppointmentsPerTestTypeAsync(ValidLocalLicenseAppId, -1));
        }

        [Fact]
        public async Task AddTestAppointmentAsync_WhenTestAppointmentIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestAppointmentRepo>>();
            var repo = new TestAppointmentRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.AddTestAppointmentAsync(null!));
        }

        [Fact]
        public async Task UpdateTestAppointmentAsync_WhenTestAppointmentIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestAppointmentRepo>>();
            var repo = new TestAppointmentRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.UpdateTestAppointmentAsync(null!));
        }

        #endregion

        #region Special Cases

        [Fact]
        public async Task GetByIdAsync_WhenDbOperationFails_ThrowException()
        {
            // Arrange
            var mockDbContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<TestAppointment>>();

            mockDbSet.Setup(x => x.FindAsync(ValidTestAppointmentId)).ThrowsAsync(new Exception("Database operation fails"));
            mockDbContext.Setup(x => x.TestAppointments).Returns(mockDbSet.Object);

            var mockLogger = new Mock<ILogger<TestAppointmentRepo>>();
            var repo = new TestAppointmentRepo(mockDbContext.Object, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await repo.GetByIdAsync(ValidTestAppointmentId));
        }

        [Fact]
        public async Task AddTestAppointmentAsync_WhenOperationTimesOut_ThrowTaskCancellationException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                  .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                  .Options;

            var mockDbContext = new Mock<AppDbContext>(options);
            var mockDbSet = new Mock<DbSet<TestAppointment>>();

            mockDbContext.Setup(x => x.TestAppointments).Returns(mockDbSet.Object);

            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new TaskCanceledException("Operation Timed Out."));

            var mockLogger = new Mock<ILogger<TestAppointmentRepo>>();
            var repo = new TestAppointmentRepo(mockDbContext.Object, mockLogger.Object);

            var testAppointment = CreateTestTestAppointment();

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(async () => await repo.AddTestAppointmentAsync(testAppointment));
        }

        #endregion

        #region Private Helpers

        private AppDbContext CreateInMemoryDbContext(TestAppointment testAppointment = null!)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test
                .Options;

            var dbContext = new AppDbContext(options);

            if (testAppointment is not null)
            {
                dbContext.TestAppointments.Add(testAppointment);
                dbContext.SaveChanges();
            }

            return dbContext;
        }

        private TestAppointment CreateTestTestAppointment() => new TestAppointment
        {
            Id = ValidTestAppointmentId,
            LocalLicenseAppId = ValidLocalLicenseAppId,
            TestTypeId = ValidTestTypeId,
            PaidFees = 10,
            TestId = 2,
            UserId = 3,
            AppointmentDate = DateTime.UtcNow,
            IsLocked = false
        };

        #endregion
    }
}