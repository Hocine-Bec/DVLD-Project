using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Interfaces;
using DVLD_DataAccess.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;
using DVLD_DataAccess.Core.Entities.Applications;
using System.ComponentModel.Design;

namespace DVLD.Tests.DataAccess
{
    public class InternationalRepoTests
    {
        private const int ValidInternationalLicenseId = 5;
        private const int ValidDriverId = 10;

        #region Positive Tests

        [Fact]
        public async Task GetByIdAsync_WhenInternationalLicenseExists_ReturnCorrectLicense()
        {
            // Arrange
            var license = CreateTestInternationalLicense();
            var context = CreateInMemoryDbContext(license);

            var mockLogger = new Mock<ILogger<InternationalRepo>>();
            var repo = new InternationalRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByIdAsync(ValidInternationalLicenseId);

            // Assert
            result.Should().BeEquivalentTo(license);
        }

        [Fact]
        public async Task GetAllInternationalLicensesAsync_WhenLicensesExist_ReturnListOfLicenses()
        {
            // Arrange
            var license = CreateTestInternationalLicense();
            var context = CreateInMemoryDbContext(license);

            var mockLogger = new Mock<ILogger<InternationalRepo>>();
            var repo = new InternationalRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllInternationalLicensesAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(license);
        }

        [Fact]
        public async Task GetDriverInternationalLicensesAsync_WhenLicensesExist_ReturnListOfLicenses()
        {
            // Arrange
            var license = CreateTestInternationalLicense();
            var context = CreateInMemoryDbContext(license);

            var mockLogger = new Mock<ILogger<InternationalRepo>>();
            var repo = new InternationalRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetDriverInternationalLicensesAsync(ValidDriverId);

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(license);
        }

        [Fact]
        public async Task AddInternationalLicenseAsync_WhenLicenseIsValid_ReturnLicenseId()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<InternationalRepo>>();
            var repo = new InternationalRepo(context, mockLogger.Object);

            var license = CreateTestInternationalLicense();

            // Act
            var result = await repo.AddInternationalLicenseAsync(license);

            // Assert
            result.Should().Be(ValidInternationalLicenseId);
        }

        [Fact]
        public async Task UpdateInternationalLicenseAsync_WhenLicenseIsValid_ReturnTrue()
        {
            // Arrange
            var license = CreateTestInternationalLicense();
            var context = CreateInMemoryDbContext(license);

            var mockLogger = new Mock<ILogger<InternationalRepo>>();
            var repo = new InternationalRepo(context, mockLogger.Object);

            // Act
            license.ExpireDate = DateTime.UtcNow.AddYears(1);
            var result = await repo.UpdateInternationalLicenseAsync(license);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetActiveInternationalLicenseIdByDriverIdAsync_WhenActiveLicenseExists_ReturnLicenseId()
        {
            // Arrange
            var license = CreateTestInternationalLicense();
            var context = CreateInMemoryDbContext(license);

            var mockLogger = new Mock<ILogger<InternationalRepo>>();
            var repo = new InternationalRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetActiveInternationalLicenseIdByDriverIdAsync(ValidDriverId);

            // Assert
            result.Should().Be(ValidInternationalLicenseId);
        }

        #endregion

        #region Negative Tests

        [Fact]
        public async Task GetByIdAsync_WhenInternationalLicenseDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<InternationalRepo>>();
            var repo = new InternationalRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByIdAsync(1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WhenInternationalLicenseIdIsZeroOrMinus_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<InternationalRepo>>();
            var repo = new InternationalRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetByIdAsync(-1));
        }

        [Fact]
        public async Task GetAllInternationalLicensesAsync_WhenNoLicensesExist_ReturnEmptyList()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<InternationalRepo>>();
            var repo = new InternationalRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllInternationalLicensesAsync();

            // Assert
            result.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetDriverInternationalLicensesAsync_WhenNoLicensesExist_ReturnEmptyList()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<InternationalRepo>>();
            var repo = new InternationalRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetDriverInternationalLicensesAsync(1);

            // Assert
            result.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetDriverInternationalLicensesAsync_WhenDriverIdIsZeroOrMinus_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<InternationalRepo>>();
            var repo = new InternationalRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetDriverInternationalLicensesAsync(-1));
        }

        [Fact]
        public async Task AddInternationalLicenseAsync_WhenLicenseIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<InternationalRepo>>();
            var repo = new InternationalRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.AddInternationalLicenseAsync(null!));
        }

        [Fact]
        public async Task UpdateInternationalLicenseAsync_WhenLicenseIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<InternationalRepo>>();
            var repo = new InternationalRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.UpdateInternationalLicenseAsync(null!));
        }

        [Fact]
        public async Task GetActiveInternationalLicenseIdByDriverIdAsync_WhenNoActiveLicenseExists_ReturnZero()
        {
            // Arrange
            var license = CreateTestInternationalLicense();
            license.ExpireDate = DateTime.UtcNow.AddYears(-1); // Expired license
            var context = CreateInMemoryDbContext(license);

            var mockLogger = new Mock<ILogger<InternationalRepo>>();
            var repo = new InternationalRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetActiveInternationalLicenseIdByDriverIdAsync(ValidDriverId);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public async Task GetActiveInternationalLicenseIdByDriverIdAsync_WhenDriverIdIsZeroOrMinus_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<InternationalRepo>>();
            var repo = new InternationalRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetActiveInternationalLicenseIdByDriverIdAsync(-1));
        }

        #endregion

        #region Special Cases

        [Fact]
        public async Task GetByIdAsync_WhenDbOperationFails_ThrowException()
        {
            // Arrange
            var mockDbContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<InternationalLicense>>();

            mockDbSet.Setup(x => x.FindAsync(ValidInternationalLicenseId)).ThrowsAsync(new Exception("Database operation fails"));
            mockDbContext.Setup(x => x.InternationalLicenses).Returns(mockDbSet.Object);

            var mockLogger = new Mock<ILogger<InternationalRepo>>();
            var repo = new InternationalRepo(mockDbContext.Object, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await repo.GetByIdAsync(ValidInternationalLicenseId));
        }

        [Fact]
        public async Task AddInternationalLicenseAsync_WhenOperationTimesOut_ThrowTaskCancellationException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                  .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                  .Options;

            var mockDbContext = new Mock<AppDbContext>(options);
            var mockDbSet = new Mock<DbSet<InternationalLicense>>();

            mockDbContext.Setup(x => x.InternationalLicenses).Returns(mockDbSet.Object);

            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new TaskCanceledException("Operation Timed Out."));

            var mockLogger = new Mock<ILogger<InternationalRepo>>();
            var repo = new InternationalRepo(mockDbContext.Object, mockLogger.Object);

            var license = CreateTestInternationalLicense();

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(async () => await repo.AddInternationalLicenseAsync(license));
        }

        #endregion

        #region Private Helpers

        private AppDbContext CreateInMemoryDbContext(InternationalLicense license = null!)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test
                .Options;

            var dbContext = new AppDbContext(options);

            if (license is not null)
            {
                dbContext.InternationalLicenses.Add(license);
                dbContext.SaveChanges();
            }

            return dbContext;
        }

        private InternationalLicense CreateTestInternationalLicense() => new InternationalLicense
        {
            Id = ValidInternationalLicenseId,
            DriverId = ValidDriverId,
            BaseAppId = 1,
            IssuedUsingLicenseId = 2,
            UserId = 3,
            IssueDate = DateTime.UtcNow,
            ExpireDate = DateTime.UtcNow.AddYears(1),
            IsActive = true


        };

        #endregion
    }
}