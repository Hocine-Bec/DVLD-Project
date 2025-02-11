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
    public class LocalLicenseAppRepoTests
    {
        private const int ValidLocalLicenseAppId = 1;
        private const int ValidAppId = 3;
        private const int ValidLicenseTypeId = 5;

        #region Positive Tests

        [Fact]
        public async Task GetByIdAsync_WhenLocalLicenseAppExists_ReturnCorrectData()
        {
            // Arrange
            var localLicenseApp = CreateTestLocalLicenseApp();
            var context = CreateInMemoryDbContext(localLicenseApp);
            var mockLogger = new Mock<ILogger<LocalLicenseAppRepo>>();
            var repo = new LocalLicenseAppRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByIdAsync(ValidLocalLicenseAppId);

            // Assert
            result.Should().Be((localLicenseApp.BaseAppId, localLicenseApp.LicenseTypeId));
        }

        [Fact]
        public async Task GetByAppIdAsync_WhenLocalLicenseAppExists_ReturnCorrectData()
        {
            // Arrange
            var localLicenseApp = CreateTestLocalLicenseApp();
            var context = CreateInMemoryDbContext(localLicenseApp);
            var mockLogger = new Mock<ILogger<LocalLicenseAppRepo>>();
            var repo = new LocalLicenseAppRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByAppIdAsync(ValidAppId);

            // Assert
            result.Should().Be((localLicenseApp.Id, localLicenseApp.LicenseTypeId));
        }

        [Fact]
        public async Task AddLocalLicenseAppAsync_WhenInputsAreValid_ReturnLocalLicenseAppId()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<LocalLicenseAppRepo>>();
            var repo = new LocalLicenseAppRepo(context, mockLogger.Object);

            // Act
            var result = await repo.AddLocalLicenseAppAsync(ValidAppId, ValidLicenseTypeId);

            // Assert
            result.Should().Be(ValidLocalLicenseAppId);
        }

        [Fact]
        public async Task UpdateLocalLicenseAppAsync_WhenInputsAreValid_ReturnTrue()
        {
            // Arrange
            var localLicenseApp = CreateTestLocalLicenseApp();
            var context = CreateInMemoryDbContext(localLicenseApp);

            var mockLogger = new Mock<ILogger<LocalLicenseAppRepo>>();
            var repo = new LocalLicenseAppRepo(context, mockLogger.Object);

            // Act
            var result = await repo.UpdateLocalLicenseAppAsync(ValidLocalLicenseAppId, ValidAppId, ValidLicenseTypeId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteLocalLicenseAppAsync_WhenLocalLicenseAppExists_ReturnTrue()
        {
            // Arrange
            var localLicenseApp = CreateTestLocalLicenseApp();
            var context = CreateInMemoryDbContext(localLicenseApp);
            var mockLogger = new Mock<ILogger<LocalLicenseAppRepo>>();
            var repo = new LocalLicenseAppRepo(context, mockLogger.Object);

            // Act
            var result = await repo.DeleteLocalLicenseAppAsync(ValidLocalLicenseAppId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetAllLocalLicenseAppAsync_WhenLocalLicenseAppsExist_ReturnListOfLocalLicenseApps()
        {
            // Arrange
            var localLicenseApp = CreateTestLocalLicenseApp();
            var context = CreateInMemoryDbContext(localLicenseApp);
            var mockLogger = new Mock<ILogger<LocalLicenseAppRepo>>();
            var repo = new LocalLicenseAppRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllLocalLicenseAppAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(localLicenseApp);
        }

        #endregion

        #region Negative Tests

        [Fact]
        public async Task GetByIdAsync_WhenLocalLicenseAppDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<LocalLicenseAppRepo>>();
            var repo = new LocalLicenseAppRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByIdAsync(ValidLocalLicenseAppId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WhenLocalLicenseAppIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<LocalLicenseAppRepo>>();
            var repo = new LocalLicenseAppRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetByIdAsync(-1));
        }

        [Fact]
        public async Task GetByAppIdAsync_WhenLocalLicenseAppDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<LocalLicenseAppRepo>>();
            var repo = new LocalLicenseAppRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByAppIdAsync(ValidAppId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByAppIdAsync_WhenAppIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<LocalLicenseAppRepo>>();
            var repo = new LocalLicenseAppRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetByAppIdAsync(-1));
        }

        [Fact]
        public async Task AddLocalLicenseAppAsync_WhenAppIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<LocalLicenseAppRepo>>();
            var repo = new LocalLicenseAppRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.AddLocalLicenseAppAsync(-1, ValidLicenseTypeId));
        }

        [Fact]
        public async Task AddLocalLicenseAppAsync_WhenLicenseTypeIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<LocalLicenseAppRepo>>();
            var repo = new LocalLicenseAppRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.AddLocalLicenseAppAsync(ValidAppId, -1));
        }

        [Fact]
        public async Task UpdateLocalLicenseAppAsync_WhenLocalLicenseAppIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<LocalLicenseAppRepo>>();
            var repo = new LocalLicenseAppRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.UpdateLocalLicenseAppAsync(-1, ValidAppId, ValidLicenseTypeId));
        }

        [Fact]
        public async Task UpdateLocalLicenseAppAsync_WhenAppIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<LocalLicenseAppRepo>>();
            var repo = new LocalLicenseAppRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.UpdateLocalLicenseAppAsync(ValidLocalLicenseAppId, -1, ValidLicenseTypeId));
        }

        [Fact]
        public async Task UpdateLocalLicenseAppAsync_WhenLicenseTypeIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<LocalLicenseAppRepo>>();
            var repo = new LocalLicenseAppRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.UpdateLocalLicenseAppAsync(ValidLocalLicenseAppId, ValidAppId, -1));
        }

        [Fact]
        public async Task DeleteLocalLicenseAppAsync_WhenLocalLicenseAppIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<LocalLicenseAppRepo>>();
            var repo = new LocalLicenseAppRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.DeleteLocalLicenseAppAsync(-1));
        }

        [Fact]
        public async Task DeleteLocalLicenseAppAsync_WhenLocalLicenseAppDoesNotExist_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<LocalLicenseAppRepo>>();
            var repo = new LocalLicenseAppRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.DeleteLocalLicenseAppAsync(ValidLocalLicenseAppId));
        }

        #endregion

        #region Special Cases

        [Fact]
        public async Task AddLocalLicenseAppAsync_WhenDbOperationFails_ThrowException()
        {
            // Arrange
            var mockDbContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<LocalLicenseApp>>();

            mockDbContext.Setup(x => x.LocalLicenses).Returns(mockDbSet.Object);
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database operation fails"));

            var mockLogger = new Mock<ILogger<LocalLicenseAppRepo>>();
            var repo = new LocalLicenseAppRepo(mockDbContext.Object, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await repo.AddLocalLicenseAppAsync(ValidAppId, ValidLicenseTypeId));
        }

        #endregion

        #region Private Helpers

        private AppDbContext CreateInMemoryDbContext(LocalLicenseApp localLicenseApp = null!)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test
                .Options;

            var dbContext = new AppDbContext(options);

            if (localLicenseApp is not null)
            {
                dbContext.LocalLicenses.Add(localLicenseApp);
                dbContext.SaveChanges();
            }

            return dbContext;
        }

        private LocalLicenseApp CreateTestLocalLicenseApp() => new LocalLicenseApp
        {
            Id = ValidLocalLicenseAppId,
            BaseAppId = ValidAppId,
            LicenseTypeId = ValidLicenseTypeId
        };

        #endregion
    }
}