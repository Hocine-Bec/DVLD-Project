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
    public class LicenseTypesRepoTests
    {
        private const int ValidLicenseClassId = 5;
        private const string ValidClassName = "Class A";

        #region Positive Tests

        [Fact]
        public async Task GetByIdAsync_WhenLicenseClassExists_ReturnCorrectLicenseClass()
        {
            // Arrange
            var licenseClass = CreateTestLicenseClass();
            var context = CreateInMemoryDbContext(licenseClass);

            var mockLogger = new Mock<ILogger<LicenseTypesRepository>>();
            var repo = new LicenseTypesRepository(context, mockLogger.Object);

            // Act
            var result = await repo.GetByIdAsync(ValidLicenseClassId);

            // Assert
            result.Should().BeEquivalentTo(licenseClass);
        }

        [Fact]
        public async Task GetByClassNameAsync_WhenLicenseClassExists_ReturnCorrectLicenseClass()
        {
            // Arrange
            var licenseClass = CreateTestLicenseClass();
            var context = CreateInMemoryDbContext(licenseClass);

            var mockLogger = new Mock<ILogger<LicenseTypesRepository>>();
            var repo = new LicenseTypesRepository(context, mockLogger.Object);

            // Act
            var result = await repo.GetByClassNameAsync(ValidClassName);

            // Assert
            result.Should().BeEquivalentTo(licenseClass);
        }

        [Fact]
        public async Task GetAllLicenseClassesAsync_WhenLicenseClassesExist_ReturnListOfLicenseClasses()
        {
            // Arrange
            var licenseClass = CreateTestLicenseClass();
            var context = CreateInMemoryDbContext(licenseClass);

            var mockLogger = new Mock<ILogger<LicenseTypesRepository>>();
            var repo = new LicenseTypesRepository(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllLicenseClassesAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(licenseClass);
        }

        [Fact]
        public async Task AddLicenseClassAsync_WhenLicenseClassIsValid_ReturnLicenseClassId()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<LicenseTypesRepository>>();
            var repo = new LicenseTypesRepository(context, mockLogger.Object);

            var licenseClass = CreateTestLicenseClass();

            // Act
            var result = await repo.AddLicenseClassAsync(licenseClass);

            // Assert
            result.Should().Be(ValidLicenseClassId);
        }

        [Fact]
        public async Task UpdateLicenseClassAsync_WhenLicenseClassIsValid_ReturnTrue()
        {
            // Arrange
            var licenseClass = CreateTestLicenseClass();
            var context = CreateInMemoryDbContext(licenseClass);

            var mockLogger = new Mock<ILogger<LicenseTypesRepository>>();
            var repo = new LicenseTypesRepository(context, mockLogger.Object);

            // Act
            licenseClass.ClassName = "Updated Class Name";
            var result = await repo.UpdateLicenseClassAsync(licenseClass);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region Negative Tests

        [Fact]
        public async Task GetByIdAsync_WhenLicenseClassDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<LicenseTypesRepository>>();
            var repo = new LicenseTypesRepository(context, mockLogger.Object);

            // Act
            var result = await repo.GetByIdAsync(1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WhenLicenseClassIdIsZeroOrMinus_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<LicenseTypesRepository>>();
            var repo = new LicenseTypesRepository(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetByIdAsync(-1));
        }

        [Fact]
        public async Task GetByClassNameAsync_WhenLicenseClassDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<LicenseTypesRepository>>();
            var repo = new LicenseTypesRepository(context, mockLogger.Object);

            // Act
            var result = await repo.GetByClassNameAsync("NonExistentClass");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByClassNameAsync_WhenClassNameIsNullOrEmpty_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<LicenseTypesRepository>>();
            var repo = new LicenseTypesRepository(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetByClassNameAsync(""));
        }

        [Fact]
        public async Task GetAllLicenseClassesAsync_WhenNoLicenseClassesExist_ReturnEmptyList()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<LicenseTypesRepository>>();
            var repo = new LicenseTypesRepository(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllLicenseClassesAsync();

            // Assert
            result.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task AddLicenseClassAsync_WhenLicenseClassIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<LicenseTypesRepository>>();
            var repo = new LicenseTypesRepository(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.AddLicenseClassAsync(null!));
        }

        [Fact]
        public async Task UpdateLicenseClassAsync_WhenLicenseClassIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<LicenseTypesRepository>>();
            var repo = new LicenseTypesRepository(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.UpdateLicenseClassAsync(null!));
        }

        #endregion

        #region Special Cases

        [Fact]
        public async Task GetByIdAsync_WhenDbOperationFails_ThrowException()
        {
            // Arrange
            var mockDbContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<LicenseType>>();

            mockDbSet.Setup(x => x.FindAsync(ValidLicenseClassId)).ThrowsAsync(new Exception("Database operation fails"));
            mockDbContext.Setup(x => x.LicenseTypes).Returns(mockDbSet.Object);

            var mockLogger = new Mock<ILogger<LicenseTypesRepository>>();
            var repo = new LicenseTypesRepository(mockDbContext.Object, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await repo.GetByIdAsync(ValidLicenseClassId));
        }

        [Fact]
        public async Task AddLicenseClassAsync_WhenOperationTimesOut_ThrowTaskCancellationException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                  .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                  .Options;

            var mockDbContext = new Mock<AppDbContext>(options);
            var mockDbSet = new Mock<DbSet<LicenseType>>();

            mockDbContext.Setup(x => x.LicenseTypes).Returns(mockDbSet.Object);

            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new TaskCanceledException("Operation Timed Out."));

            var mockLogger = new Mock<ILogger<LicenseTypesRepository>>();
            var repo = new LicenseTypesRepository(mockDbContext.Object, mockLogger.Object);

            var licenseClass = CreateTestLicenseClass();

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(async () => await repo.AddLicenseClassAsync(licenseClass));
        }

        #endregion

        #region Private Helpers

        private AppDbContext CreateInMemoryDbContext(LicenseType licenseClass = null!)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test
                .Options;

            var dbContext = new AppDbContext(options);

            if (licenseClass is not null)
            {
                dbContext.LicenseTypes.Add(licenseClass);
                dbContext.SaveChanges();
            }

            return dbContext;
        }

        private LicenseType CreateTestLicenseClass() => new LicenseType
        {
            Id = ValidLicenseClassId,
            ClassName = ValidClassName,
            ClassFees = 10,
            DefaultValidityLength = 10,
            ClassDescription = "Test Description",
            MinimumAllowedAge = 18
        };

        #endregion
    }
}