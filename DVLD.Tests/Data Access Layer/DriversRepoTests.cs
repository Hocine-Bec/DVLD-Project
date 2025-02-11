using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;
using DVLD_DataAccess.Core.Repositories;
using DVLD_DataAccess.Core.Entities.Identity;

namespace DVLD.Tests.DataAccess
{
    public class DriversRepoTests
    {
        private const int ValidPersonId = 5;
        private const int ValidUserId = 10;
        private const int ValidDriverId = 1;

        #region Positive Tests

        [Fact]
        public async Task AddDriverAsync_WhenValidPersonIdAndUserId_ReturnDriverId()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<DriversRepo>>();
            var repo = new DriversRepo(context, mockLogger.Object);

            // Act
            var result = await repo.AddDriverAsync(ValidPersonId, ValidUserId);

            // Assert
            result.Should().BePositive();
        }

        [Fact]
        public async Task GetAllDriversAsync_WhenDriversExist_ReturnListOfDrivers()
        {
            // Arrange
            var driver = CreateTestDriver();
            var context = CreateInMemoryDbContext(driver);
            var mockLogger = new Mock<ILogger<DriversRepo>>();
            var repo = new DriversRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllDriversAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(driver);
        }

        [Fact]
        public async Task GetByDriverIdAsync_WhenDriverExists_ReturnCorrectDriver()
        {
            // Arrange
            var driver = CreateTestDriver();
            var context = CreateInMemoryDbContext(driver);
            var mockLogger = new Mock<ILogger<DriversRepo>>();
            var repo = new DriversRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByDriverIdAsync(ValidDriverId);

            // Assert
            result.Should().BeEquivalentTo(driver);
        }

        [Fact]
        public async Task GetByPersonIdAsync_WhenDriverExistsWithPersonId_ReturnCorrectDriver()
        {
            // Arrange
            var driver = CreateTestDriver();
            var context = CreateInMemoryDbContext(driver);
            var mockLogger = new Mock<ILogger<DriversRepo>>();
            var repo = new DriversRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByPersonIdAsync(ValidPersonId);

            // Assert
            result.Should().BeEquivalentTo(driver);
        }

        [Fact]
        public async Task UpdateDriverAsync_WhenDriverIsValid_ReturnTrue()
        {
            // Arrange
            var driver = CreateTestDriver();
            var context = CreateInMemoryDbContext(driver);
            var mockLogger = new Mock<ILogger<DriversRepo>>();
            var repo = new DriversRepo(context, mockLogger.Object);

            // Act
            driver.CreatedDate = DateTime.UtcNow.AddDays(-1); // Modify a field
            var result = await repo.UpdateDriverAsync(driver);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region Negative Tests

        [Fact]
        public async Task AddDriverAsync_WhenPersonIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<DriversRepo>>();
            var repo = new DriversRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.AddDriverAsync(-1, ValidUserId));
        }

        [Fact]
        public async Task AddDriverAsync_WhenUserIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<DriversRepo>>();
            var repo = new DriversRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.AddDriverAsync(ValidPersonId, -1));
        }

        [Fact]
        public async Task GetByDriverIdAsync_WhenDriverDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<DriversRepo>>();
            var repo = new DriversRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByDriverIdAsync(ValidDriverId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByDriverIdAsync_WhenDriverIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<DriversRepo>>();
            var repo = new DriversRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetByDriverIdAsync(-1));
        }

        [Fact]
        public async Task GetByPersonIdAsync_WhenDriverDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<DriversRepo>>();
            var repo = new DriversRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByPersonIdAsync(ValidPersonId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByPersonIdAsync_WhenPersonIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<DriversRepo>>();
            var repo = new DriversRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetByPersonIdAsync(-1));
        }

        [Fact]
        public async Task UpdateDriverAsync_WhenDriverIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<DriversRepo>>();
            var repo = new DriversRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.UpdateDriverAsync(null!));
        }

        #endregion

        #region Special Cases

        [Fact]
        public async Task AddDriverAsync_WhenDbOperationFails_ThrowException()
        {
            // Arrange
            var mockDbContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<Driver>>();

            mockDbContext.Setup(x => x.Drivers).Returns(mockDbSet.Object);
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database operation fails"));

            var mockLogger = new Mock<ILogger<DriversRepo>>();
            var repo = new DriversRepo(mockDbContext.Object, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await repo.AddDriverAsync(ValidPersonId, ValidUserId));
        }

        [Fact]
        public async Task UpdateDriverAsync_WhenDbOperationFails_ThrowException()
        {
            // Arrange
            var driver = CreateTestDriver();
            var mockDbContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<Driver>>();

            mockDbContext.Setup(x => x.Drivers).Returns(mockDbSet.Object);
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database operation fails"));

            var mockLogger = new Mock<ILogger<DriversRepo>>();
            var repo = new DriversRepo(mockDbContext.Object, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await repo.UpdateDriverAsync(driver));
        }

        #endregion

        #region Private Helpers

        private AppDbContext CreateInMemoryDbContext(Driver driver = null!)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test
                .Options;

            var dbContext = new AppDbContext(options);

            if (driver is not null)
            {
                dbContext.Drivers.Add(driver);
                dbContext.SaveChanges();
            }

            return dbContext;
        }

        private Driver CreateTestDriver() => new Driver
        {
            Id = ValidDriverId,
            PersonId = ValidPersonId,
            UserId = ValidUserId,
            CreatedDate = DateTime.UtcNow
        };

        #endregion
    }
}