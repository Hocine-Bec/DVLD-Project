using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;
using DVLD_DataAccess.Core.Entities.Applications;
using DVLD_DataAccess.Core.Enums;
using DVLD_DataAccess.Core.Repositories;
using Microsoft.Data.Sqlite;
using DVLD_DataAccess.Core.Entities.Identity;


namespace DVLD.Tests.DataAccess
{
    public class BaseAppRepoTests
    {
        private const int ValidAppId = 1;
        private const int InvalidAppId = -1;
        private const int ValidPersonId = 1;
        private const int ValidAppTypeId = 1;
        private const int ValidLicenseTypeId = 1;
        private const short ValidStatus = 1;
        private const short InvalidStatus = -1;

        #region Positive Tests

        [Fact]
        public async Task AddBaseAppAsync_WhenBaseAppIsValid_ReturnAppId()
        {
            // Arrange
            var baseApp = CreateTestBaseApp();
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(context, mockLogger.Object);

            // Act
            var result = await repo.AddBaseAppAsync(baseApp);

            // Assert
            result.Should().Be(ValidAppId);
        }

        [Fact]
        public async Task DeleteBaseAppAsync_WhenBaseAppExists_ReturnTrue()
        {
            // Arrange
            var baseApp = CreateTestBaseApp();
            var context = CreateInMemoryDbContext(baseApp);
            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(context, mockLogger.Object);

            // Act
            var result = await repo.DeleteBaseAppAsync(ValidAppId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DoesBaseAppExistAsync_WhenBaseAppExists_ReturnTrue()
        {
            // Arrange
            var baseApp = CreateTestBaseApp();
            var context = CreateInMemoryDbContext(baseApp);
            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(context, mockLogger.Object);

            // Act
            var result = await repo.DoesBaseAppExistAsync(ValidAppId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DoesPersonHaveActiveBaseApp_WhenPersonHasActiveBaseApp_ReturnTrue()
        {
            // Arrange
            var baseApp = CreateTestBaseApp();
            var context = CreateInMemoryDbContext(baseApp);
            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(context, mockLogger.Object);

            // Act
            var result = await repo.DoesPersonHaveActiveBaseAppAsync(ValidPersonId, ValidAppTypeId);

            // Assert
            result.Should().BeTrue();
        }


        //[Fact]
        //public async Task GetActiveAppIdByLicenseTypeAsync_WhenActiveAppExists_ReturnAppId()
        //{
        //    // Arrange
        //    var baseApp = CreateTestBaseApp();
        //    var context = CreateInMemoryDbContext(baseApp);
        //    var mockLogger = new Mock<ILogger<BaseAppRepo>>();
        //    var repo = new BaseAppRepo(context, mockLogger.Object);

        //    // Act
        //    var result = await repo.GetActiveAppIdByLicenseTypeAsync(ValidPersonId, ValidAppTypeId, ValidLicenseTypeId);

        //    // Assert
        //    result.Should().Be(ValidAppId);
        //}

        //[Fact]
        //public async Task GetActiveAppIdByLicenseTypeAsync_WhenActiveAppExists_ReturnAppId()
        //{
        //    // Arrange
        //    using var connection = new SqliteConnection("DataSource=:memory:");
        //    connection.Open();

        //    var options = new DbContextOptionsBuilder<AppDbContext>()
        //        .UseSqlite(connection)
        //        .Options;

        //    using (var context = new AppDbContext(options))
        //    {
        //        context.Database.EnsureCreated();

        //        var baseApp = CreateTestBaseApp();

        //        context.BaseApps.Add(baseApp);
        //        await context.SaveChangesAsync();

        //        var mockLogger = new Mock<ILogger<BaseAppRepo>>();
        //        var repo = new BaseAppRepo(context, mockLogger.Object);

        //        // Act
        //        var result = await repo.GetActiveAppIdByLicenseTypeAsync(ValidPersonId, ValidAppTypeId, ValidLicenseTypeId);

        //        // Assert
        //        result.Should().Be(ValidAppId);
        //    }
        //}

        [Fact]
        public async Task GetActiveAppIdByLicenseTypeAsync_WhenActiveAppExists_ReturnAppId()
        {
            // Arrange
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new AppDbContext(options))
            {
                // Ensure the database schema is created
                context.Database.EnsureCreated();

                // Create and insert related entities
                var person = new Person 
                { 
                    Id = ValidPersonId, 
                    FirstName = "Test", 
                    CountryId =1, 
                    LastName = "Test",
                    NationalNo = "N1",
                    DateOfBirth = new DateTime(2000, 1, 1)                 
                };
                var appType = new AppType { Id = ValidAppTypeId, Title = "Test App Type", Fees = 10 };

                context.People.Add(person);
                await context.SaveChangesAsync();

                context.ApplicationTypes.Add(appType);
                await context.SaveChangesAsync();

                // Create and insert the main entity (BaseApp)
                var baseApp = CreateTestBaseApp();
                context.BaseApps.Add(baseApp);
                await context.SaveChangesAsync();

                var mockLogger = new Mock<ILogger<BaseAppRepo>>();
                var repo = new BaseAppRepo(context, mockLogger.Object);

                // Act
                var result = await repo.GetActiveAppIdByLicenseTypeAsync(ValidPersonId, ValidAppTypeId, ValidLicenseTypeId);

                // Assert
                result.Should().Be(ValidAppId);
            }
        }

        [Fact]
        public async Task GetActiveBaseAppIdAsync_WhenActiveBaseAppExists_ReturnAppId()
        {
            // Arrange
            var baseApp = CreateTestBaseApp();
            var context = CreateInMemoryDbContext(baseApp);
            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetActiveBaseAppIdAsync(ValidPersonId, ValidAppTypeId);

            // Assert
            result.Should().Be(ValidAppId);
        }

        [Fact]
        public async Task GetAllBaseAppsAsync_WhenBaseAppsExist_ReturnListOfBaseApps()
        {
            // Arrange
            var baseApp = CreateTestBaseApp();
            var context = CreateInMemoryDbContext(baseApp);
            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllBaseAppsAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(baseApp);
        }

        [Fact]
        public async Task GetByBaseAppIdIdAsync_WhenBaseAppExists_ReturnCorrectBaseApp()
        {
            // Arrange
            var baseApp = CreateTestBaseApp();
            var context = CreateInMemoryDbContext(baseApp);
            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByBaseAppIdIdAsync(ValidAppId);

            // Assert
            result.Should().BeEquivalentTo(baseApp);
        }

        [Fact]
        public async Task UpdateBaseAppAsync_WhenBaseAppIsValid_ReturnTrue()
        {
            // Arrange
            var baseApp = CreateTestBaseApp();
            var context = CreateInMemoryDbContext(baseApp);
            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(context, mockLogger.Object);

            // Act
            baseApp.Status = AppStatus.Completed;
            var result = await repo.UpdateBaseAppAsync(baseApp);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenStatusIsValid_ReturnTrue()
        {
            // Arrange
            var baseApp = CreateTestBaseApp();
            var context = CreateInMemoryDbContext(baseApp);
            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(context, mockLogger.Object);

            // Act
            var result = await repo.UpdateStatusAsync(ValidAppId, ValidStatus);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region Negative Tests

        [Fact]
        public async Task AddBaseAppAsync_WhenBaseAppIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.AddBaseAppAsync(null!));
        }

        [Fact]
        public async Task DeleteBaseAppAsync_WhenBaseAppDoesNotExist_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.DeleteBaseAppAsync(ValidAppId));
        }

        [Fact]
        public async Task DeleteBaseAppAsync_WhenAppIdIsZeroOrNegative_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.DeleteBaseAppAsync(InvalidAppId));
        }

        [Fact]
        public async Task DoesBaseAppExistAsync_WhenAppIdIsZeroOrNegative_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.DoesBaseAppExistAsync(InvalidAppId));
        }

        [Fact]
        public async Task GetActiveBaseAppIdAsync_WhenAppTypeIdIsZeroOrNegative_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetActiveBaseAppIdAsync(ValidPersonId, InvalidAppId));
        }

        [Fact]
        public async Task GetByBaseAppIdIdAsync_WhenAppIdIsZeroOrNegative_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetByBaseAppIdIdAsync(InvalidAppId));
        }

        [Fact]
        public async Task UpdateBaseAppAsync_WhenBaseAppIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.UpdateBaseAppAsync(null!));
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenAppIdIsZeroOrNegative_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.UpdateStatusAsync(InvalidAppId, ValidStatus));
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenStatusIsZeroOrNegative_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.UpdateStatusAsync(ValidAppId, InvalidStatus));
        }

        #endregion

        #region Special Cases

        [Fact]
        public async Task GetByBaseAppIdIdAsync_WhenDbOperationFails_ThrowException()
        {
            // Arrange
            var mockDbContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<BaseApp>>();

            mockDbSet.Setup(x => x.FindAsync(ValidAppId)).ThrowsAsync(new Exception("Database operation fails"));
            mockDbContext.Setup(x => x.BaseApps).Returns(mockDbSet.Object);

            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(mockDbContext.Object, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await repo.GetByBaseAppIdIdAsync(ValidAppId));
        }

        [Fact]
        public async Task AddBaseAppAsync_WhenOperationTimesOut_ThrowTaskCancellationException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var mockDbContext = new Mock<AppDbContext>(options);
            var mockDbSet = new Mock<DbSet<BaseApp>>();

            mockDbContext.Setup(x => x.BaseApps).Returns(mockDbSet.Object);

            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new TaskCanceledException("Operation Timed Out."));

            var mockLogger = new Mock<ILogger<BaseAppRepo>>();
            var repo = new BaseAppRepo(mockDbContext.Object, mockLogger.Object);

            var baseApp = CreateTestBaseApp();

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(async () => await repo.AddBaseAppAsync(baseApp));
        }

        #endregion

        #region Private Helpers

        private AppDbContext CreateInMemoryDbContext(BaseApp baseApp = null!)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test
                .Options;

            var dbContext = new AppDbContext(options);

            if (baseApp is not null)
            {
                dbContext.BaseApps.Add(baseApp);
                dbContext.SaveChanges();
            }

            return dbContext;
        }

        private BaseApp CreateTestBaseApp() => new BaseApp
        {
            Id = ValidAppId,
            AppDate = DateTime.UtcNow,
            PaidFees = 10,
            UserId = 1,
            PersonId = ValidPersonId,
            AppTypeId = ValidAppTypeId,
            Status = AppStatus.New
        };

        #endregion
    }
}