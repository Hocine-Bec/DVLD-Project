using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;
using DVLD_DataAccess.Core.Entities.Applications;
using DVLD_DataAccess.Core.Repositories;

namespace DVLD.Tests.DataAccess
{
    public class DetainedLicensesRepoTests
    {
        private const int ValidDetainId = 2;
        private const int ValidLicenseId = 2;
        private const int ValidUserId = 3;
        private const int ValidReleaseAppId = 4;

        #region Positive Tests

        [Fact]
        public async Task AddDetainedLicenseAsync_WhenDetainedLicenseIsValid_ReturnDetainId()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<DetainedLicensesRepo>>();
            var repo = new DetainedLicensesRepo(context, mockLogger.Object);

            var detainedLicense = CreateTestDetainedLicense();

            // Act
            var result = await repo.AddDetainedLicenseAsync(detainedLicense);

            // Assert
            result.Should().Be(ValidDetainId);
        }

        [Fact]
        public async Task GetAllDetainedLicensesAsync_WhenDetainedLicensesExist_ReturnListOfDetainedLicenses()
        {
            // Arrange
            var detainedLicense = CreateTestDetainedLicense();
            var context = CreateInMemoryDbContext(detainedLicense);

            var mockLogger = new Mock<ILogger<DetainedLicensesRepo>>();
            var repo = new DetainedLicensesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllDetainedLicensesAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(detainedLicense);
        }

        [Fact]
        public async Task GetByIdAsync_WhenDetainedLicenseExists_ReturnDetainedLicense()
        {
            // Arrange
            var detainedLicense = CreateTestDetainedLicense();
            var context = CreateInMemoryDbContext(detainedLicense);

            var mockLogger = new Mock<ILogger<DetainedLicensesRepo>>();
            var repo = new DetainedLicensesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByIdAsync(ValidDetainId);

            // Assert
            result.Should().BeEquivalentTo(detainedLicense);
        }

        [Fact]
        public async Task GetByLicenseIdAsync_WhenDetainedLicenseExists_ReturnDetainedLicense()
        {
            // Arrange
            var detainedLicense = CreateTestDetainedLicense();
            var context = CreateInMemoryDbContext(detainedLicense);

            var mockLogger = new Mock<ILogger<DetainedLicensesRepo>>();
            var repo = new DetainedLicensesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByLicenseIdAsync(ValidLicenseId);

            // Assert
            result.Should().BeEquivalentTo(detainedLicense);
        }

        [Fact]
        public async Task IsLicenseDetainedAsync_WhenLicenseIsDetained_ReturnTrue()
        {
            // Arrange
            var detainedLicense = CreateTestDetainedLicense();
            var context = CreateInMemoryDbContext(detainedLicense);

            var mockLogger = new Mock<ILogger<DetainedLicensesRepo>>();
            var repo = new DetainedLicensesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.IsLicenseDetainedAsync(ValidLicenseId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ReleaseDetainedLicenseAsync_WhenDetainedLicenseExists_ReturnTrue()
        {
            // Arrange
            var detainedLicense = CreateTestDetainedLicense();
            var context = CreateInMemoryDbContext(detainedLicense);

            var mockLogger = new Mock<ILogger<DetainedLicensesRepo>>();
            var repo = new DetainedLicensesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.ReleaseDetainedLicenseAsync(ValidDetainId, ValidUserId, ValidReleaseAppId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateDetainedLicenseAsync_WhenDetainedLicenseIsValid_ReturnTrue()
        {
            // Arrange
            var detainedLicense = CreateTestDetainedLicense();
            var context = CreateInMemoryDbContext(detainedLicense);

            var mockLogger = new Mock<ILogger<DetainedLicensesRepo>>();
            var repo = new DetainedLicensesRepo(context, mockLogger.Object);

            // Act
            detainedLicense.IsReleased = true;
            var result = await repo.UpdateDetainedLicenseAsync(detainedLicense);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region Negative Tests

        [Fact]
        public async Task AddDetainedLicenseAsync_WhenDetainedLicenseIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<DetainedLicensesRepo>>();
            var repo = new DetainedLicensesRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.AddDetainedLicenseAsync(null!));
        }

        [Fact]
        public async Task GetByIdAsync_WhenDetainedLicenseDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<DetainedLicensesRepo>>();
            var repo = new DetainedLicensesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByIdAsync(ValidDetainId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByLicenseIdAsync_WhenDetainedLicenseDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<DetainedLicensesRepo>>();
            var repo = new DetainedLicensesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByLicenseIdAsync(ValidLicenseId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task ReleaseDetainedLicenseAsync_WhenDetainedLicenseDoesNotExist_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<DetainedLicensesRepo>>();
            var repo = new DetainedLicensesRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.ReleaseDetainedLicenseAsync(ValidDetainId, ValidUserId, ValidReleaseAppId));
        }

        [Fact]
        public async Task UpdateDetainedLicenseAsync_WhenDetainedLicenseIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<DetainedLicensesRepo>>();
            var repo = new DetainedLicensesRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.UpdateDetainedLicenseAsync(null!));
        }

        #endregion

        #region Special Cases

        [Fact]
        public async Task GetByIdAsync_WhenDbOperationFails_ThrowException()
        {
            // Arrange
            var mockDbContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<DetainedLicense>>();

            mockDbSet.Setup(x => x.FindAsync(ValidDetainId)).ThrowsAsync(new Exception("Database operation fails"));
            mockDbContext.Setup(x => x.DetainedLicenses).Returns(mockDbSet.Object);

            var mockLogger = new Mock<ILogger<DetainedLicensesRepo>>();
            var repo = new DetainedLicensesRepo(mockDbContext.Object, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await repo.GetByIdAsync(ValidDetainId));
        }

        [Fact]
        public async Task AddDetainedLicenseAsync_WhenOperationTimesOut_ThrowTaskCancellationException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var mockDbContext = new Mock<AppDbContext>(options);
            var mockDbSet = new Mock<DbSet<DetainedLicense>>();

            mockDbContext.Setup(x => x.DetainedLicenses).Returns(mockDbSet.Object);

            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new TaskCanceledException("Operation Timed Out."));

            var mockLogger = new Mock<ILogger<DetainedLicensesRepo>>();
            var repo = new DetainedLicensesRepo(mockDbContext.Object, mockLogger.Object);

            var detainedLicense = CreateTestDetainedLicense();

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(async () => await repo.AddDetainedLicenseAsync(detainedLicense));
        }

        #endregion

        #region Private Helpers

        private AppDbContext CreateInMemoryDbContext(DetainedLicense detainedLicense = null!)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new AppDbContext(options);

            if (detainedLicense is not null)
            {
                dbContext.DetainedLicenses.Add(detainedLicense);
                dbContext.SaveChanges();
            }

            return dbContext;
        }

        private DetainedLicense CreateTestDetainedLicense() => new DetainedLicense
        {
            Id = ValidDetainId,
            LicenseId = ValidLicenseId,
            DetainDate = DateTime.UtcNow,
            IsReleased = false,
            FineFees = 10,
            ReleasedByUserId = ValidUserId,
            ReleaseAppId = ValidReleaseAppId
        };

        #endregion
    }
}