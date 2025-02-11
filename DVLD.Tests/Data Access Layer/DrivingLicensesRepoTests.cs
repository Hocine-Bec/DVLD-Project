using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Enums;
using DVLD_DataAccess.Core.Repositories;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace DVLD.Tests.DataAccess
{

    public class DrivingLicensesRepoTests
    {
        private const int ValidLicenseId = 1;
        private const int ValidDriverId = 1;
        private const int ValidPersonId = 1;
        private const int ValidLicenseClassId = 1;

        #region Positive Tests

        [Fact]
        public async Task GetByLicenseIdAsync_WhenLicenseExists_ReturnCorrectLicense()
        {
            // Arrange
            var license = CreateTestLicense();
            var context = CreateInMemoryDbContext(license);

            var mockLogger = new Mock<ILogger<DrivingLicensesRepo>>();
            var repo = new DrivingLicensesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByLicenseIdAsync(ValidLicenseId);

            // Assert
            result.Should().BeEquivalentTo(license);
        }

        [Fact]
        public async Task GetAllLicensesAsync_WhenLicensesExist_ReturnListOfLicenses()
        {
            // Arrange
            var license = CreateTestLicense();
            var context = CreateInMemoryDbContext(license);
            var mockLogger = new Mock<ILogger<DrivingLicensesRepo>>();
            var repo = new DrivingLicensesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllLicensesAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(license);
        }

        [Fact]
        public async Task GetDriverLicensesAsync_WhenDriverHasLicenses_ReturnListOfLicenses()
        {
            // Arrange
            var license = CreateTestLicense();
            var context = CreateInMemoryDbContext(license);
            var mockLogger = new Mock<ILogger<DrivingLicensesRepo>>();
            var repo = new DrivingLicensesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetDriverLicensesAsync(ValidDriverId);

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(license);
        }

        [Fact]
        public async Task AddLicenseAsync_WhenLicenseIsValid_ReturnLicenseId()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<DrivingLicensesRepo>>();
            var repo = new DrivingLicensesRepo(context, mockLogger.Object);
            var license = CreateTestLicense();

            // Act
            var result = await repo.AddLicenseAsync(license);

            // Assert
            result.Should().Be(ValidLicenseId);
        }

        [Fact]
        public async Task UpdateLicenseAsync_WhenLicenseIsValid_ReturnTrue()
        {
            // Arrange
            var license = CreateTestLicense();
            var context = CreateInMemoryDbContext(license);
            var mockLogger = new Mock<ILogger<DrivingLicensesRepo>>();
            var repo = new DrivingLicensesRepo(context, mockLogger.Object);

            // Act
            license.IsActive = false; // Modify a field
            var result = await repo.UpdateLicenseAsync(license);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetActiveLicenseIdByPersonIdAsync_WhenActiveLicenseExists_ReturnLicenseId()
        {
            // Arrange
            var license = CreateTestLicense();
            var context = CreateInMemoryDbContext(license);
            var mockLogger = new Mock<ILogger<DrivingLicensesRepo>>();
            var repo = new DrivingLicensesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetActiveLicenseIdByPersonIdAsync(ValidPersonId, ValidLicenseClassId);

            // Assert
            result.Should().Be(ValidLicenseId);
        }

        [Fact]
        public async Task DeactivateLicenseAsync_WhenLicenseExists_ReturnTrue()
        {
            // Arrange
            var license = CreateTestLicense();
            var context = CreateInMemoryDbContext(license);
            var mockLogger = new Mock<ILogger<DrivingLicensesRepo>>();
            var repo = new DrivingLicensesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.DeactivateLicenseAsync(ValidLicenseId);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region Negative Tests

        [Fact]
        public async Task GetByLicenseIdAsync_WhenLicenseDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<DrivingLicensesRepo>>();
            var repo = new DrivingLicensesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByLicenseIdAsync(ValidLicenseId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByLicenseIdAsync_WhenLicenseIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<DrivingLicensesRepo>>();
            var repo = new DrivingLicensesRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetByLicenseIdAsync(-1));
        }

        [Fact]
        public async Task GetDriverLicensesAsync_WhenDriverIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<DrivingLicensesRepo>>();
            var repo = new DrivingLicensesRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetDriverLicensesAsync(-1));
        }

        [Fact]
        public async Task AddLicenseAsync_WhenLicenseIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<DrivingLicensesRepo>>();
            var repo = new DrivingLicensesRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.AddLicenseAsync(null!));
        }

        [Fact]
        public async Task UpdateLicenseAsync_WhenLicenseIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<DrivingLicensesRepo>>();
            var repo = new DrivingLicensesRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.UpdateLicenseAsync(null!));
        }

        [Fact]
        public async Task GetActiveLicenseIdByPersonIdAsync_WhenPersonIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<DrivingLicensesRepo>>();
            var repo = new DrivingLicensesRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetActiveLicenseIdByPersonIdAsync(-1, ValidLicenseClassId));
        }

        [Fact]
        public async Task GetActiveLicenseIdByPersonIdAsync_WhenLicenseClassIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<DrivingLicensesRepo>>();
            var repo = new DrivingLicensesRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetActiveLicenseIdByPersonIdAsync(ValidPersonId, -1));
        }

        [Fact]
        public async Task DeactivateLicenseAsync_WhenLicenseIdIsInvalid_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<DrivingLicensesRepo>>();
            var repo = new DrivingLicensesRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.DeactivateLicenseAsync(-1));
        }

        #endregion

        #region Special Cases

        [Fact]
        public async Task AddLicenseAsync_WhenDbOperationFails_ThrowException()
        {
            // Arrange
            var mockDbContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<DrivingLicense>>();

            mockDbContext.Setup(x => x.DrivingLicenses).Returns(mockDbSet.Object);
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database operation fails"));

            var mockLogger = new Mock<ILogger<DrivingLicensesRepo>>();
            var repo = new DrivingLicensesRepo(mockDbContext.Object, mockLogger.Object);
            var license = CreateTestLicense();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await repo.AddLicenseAsync(license));
        }

        [Fact]
        public async Task UpdateLicenseAsync_WhenDbOperationFails_ThrowException()
        {
            // Arrange
            var license = CreateTestLicense();
            var mockDbContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<DrivingLicense>>();

            mockDbContext.Setup(x => x.DrivingLicenses).Returns(mockDbSet.Object);
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database operation fails"));

            var mockLogger = new Mock<ILogger<DrivingLicensesRepo>>();
            var repo = new DrivingLicensesRepo(mockDbContext.Object, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await repo.UpdateLicenseAsync(license));
        }

        #endregion

        #region Private Helpers

        private AppDbContext CreateInMemoryDbContext(DrivingLicense license = null!)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                  .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test
                  .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            if (license is not null)
            {
                context.DrivingLicenses.Add(license);
                context.SaveChanges();
            }

            return context;
        }

        private DrivingLicense CreateTestLicense() => new DrivingLicense
        {
            Id = ValidLicenseId,
            DriverId = ValidDriverId,
            LicenseTypeId = ValidLicenseClassId,
            IssueDate = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddYears(1),
            BaseAppId = 1,
            IssueReason = (IssueReason)1,
            PaidFees = 10,
            UserId = 2,
            IsActive = true
        };

        #endregion
    }
    
}
