using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;
using DVLD_DataAccess.Core.Repositories;
using DVLD_DataAccess.Core.Entities.Applications;

namespace DVLD.Tests.DataAccess
{
    public class AppTypeRepoTests
    {
        private const int ValidAppTypeId = 1;
        private const int InvalidAppTypeId = -1;

        #region Positive Tests

        [Fact]
        public async Task AddAppTypeAsync_WhenAppTypeIsValid_ReturnAppTypeId()
        {
            // Arrange
            var appType = CreateTestAppType();
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<AppTypesRepo>>();
            var repo = new AppTypesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.AddAppTypeAsync(appType);

            // Assert
            result.Should().Be(ValidAppTypeId);
        }

        [Fact]
        public async Task GetAllAppTypesAsync_WhenAppTypesExist_ReturnListOfAppTypes()
        {
            // Arrange
            var appType = CreateTestAppType();
            var context = CreateInMemoryDbContext(appType);
            var mockLogger = new Mock<ILogger<AppTypesRepo>>();
            var repo = new AppTypesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllAppTypesAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(appType);
        }

        [Fact]
        public async Task GetAllAppTypesAsync_WhenLargeDataSet_ReturnAllAppTypes()
        {
            //Arrange
            var appTypes = Enumerable.Range(1, 1000).Select(i => new AppType()
            {
                Id = i,
                Title = $"TestType {i}",
                Fees = i + 10
            }).ToList();

            var context = CreateInMemoryDbContext();

            context.ApplicationTypes.AddRange(appTypes);
            await context.SaveChangesAsync();

            var mockLogger = new Mock<ILogger<AppTypesRepo>>();
            var repo = new AppTypesRepo(context, mockLogger.Object);

            //Act
            var result = await repo.GetAllAppTypesAsync();

            //Assert
            result.Should().HaveCount(1000);
        }

        [Fact]
        public async Task GetByIdAsync_WhenAppTypeExists_ReturnCorrectAppType()
        {
            // Arrange
            var appType = CreateTestAppType();
            var context = CreateInMemoryDbContext(appType);
            var mockLogger = new Mock<ILogger<AppTypesRepo>>();
            var repo = new AppTypesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByIdAsync(ValidAppTypeId);

            // Assert
            result.Should().BeEquivalentTo(appType);
        }

        [Fact]
        public async Task UpdateAppTypeAsync_WhenAppTypeIsValid_ReturnTrue()
        {
            // Arrange
            var appType = CreateTestAppType();
            var context = CreateInMemoryDbContext(appType);
            var mockLogger = new Mock<ILogger<AppTypesRepo>>();
            var repo = new AppTypesRepo(context, mockLogger.Object);

            // Act
            appType.Title = "UpdatedTypeName";
            var result = await repo.UpdateAppTypeAsync(appType);

            // Assert
            result.Should().BeTrue();
        }

        #endregion


        #region Negative Tests

        [Fact]
        public async Task AddAppTypeAsync_WhenAppTypeIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<AppTypesRepo>>();
            var repo = new AppTypesRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.AddAppTypeAsync(null!));
        }

        [Fact]
        public async Task GetByIdAsync_WhenAppTypeDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<AppTypesRepo>>();
            var repo = new AppTypesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByIdAsync(ValidAppTypeId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WhenAppTypeIdIsZeroOrNegative_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<AppTypesRepo>>();
            var repo = new AppTypesRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetByIdAsync(InvalidAppTypeId));
        }

        [Fact]
        public async Task UpdateAppTypeAsync_WhenAppTypeIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<AppTypesRepo>>();
            var repo = new AppTypesRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.UpdateAppTypeAsync(null!));
        }

        [Fact]
        public async Task GetAllAppTypesAsync_WhenNoAppTypesExist_ReturnEmptyList()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<AppTypesRepo>>();
            var repo = new AppTypesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllAppTypesAsync();

            // Assert
            result.Should().BeNullOrEmpty();
        }

        #endregion

        #region Special Cases

        [Fact]
        public async Task GetByIdAsync_WhenDbOperationFails_ThrowException()
        {
            // Arrange
            var mockDbContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<AppType>>();

            mockDbSet.Setup(x => x.FindAsync(ValidAppTypeId)).ThrowsAsync(new Exception("Database operation fails"));
            mockDbContext.Setup(x => x.ApplicationTypes).Returns(mockDbSet.Object);

            var mockLogger = new Mock<ILogger<AppTypesRepo>>();
            var repo = new AppTypesRepo(mockDbContext.Object, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await repo.GetByIdAsync(ValidAppTypeId));
        }

        [Fact]
        public async Task AddAppTypeAsync_WhenOperationTimesOut_ThrowTaskCancellationException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var mockDbContext = new Mock<AppDbContext>(options);
            var mockDbSet = new Mock<DbSet<AppType>>();

            mockDbContext.Setup(x => x.ApplicationTypes).Returns(mockDbSet.Object);

            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new TaskCanceledException("Operation Timed Out."));

            var mockLogger = new Mock<ILogger<AppTypesRepo>>();
            var repo = new AppTypesRepo(mockDbContext.Object, mockLogger.Object);

            var appType = CreateTestAppType();

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(async () => await repo.AddAppTypeAsync(appType));
        }

        #endregion

        #region Private Helpers

        private AppDbContext CreateInMemoryDbContext(AppType? appType = null!)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test
                .Options;

            var dbContext = new AppDbContext(options);

            if (appType is not null)
            {
                dbContext.ApplicationTypes.Add(appType);
                dbContext.SaveChanges();
            }

            return dbContext;
        }

        private AppType CreateTestAppType() => new AppType
        {
            Id = ValidAppTypeId,
            Title = "TestType",
            Fees = 15
        };

        #endregion
    }
}