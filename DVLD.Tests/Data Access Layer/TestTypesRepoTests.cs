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
    public class TestTypesRepoTests
    {
        private const int ValidTestTypeId = 5;
        private const string ValidTestTypeTitle = "Vision Test";

        #region Positive Tests

        [Fact]
        public async Task GetByIdAsync_WhenTestTypeExists_ReturnCorrectTestType()
        {
            // Arrange
            var testType = CreateTestTestType();
            var context = CreateInMemoryDbContext(testType);

            var mockLogger = new Mock<ILogger<TestTypesRepo>>();
            var repo = new TestTypesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByIdAsync(ValidTestTypeId);

            // Assert
            result.Should().BeEquivalentTo(testType);
        }

        [Fact]
        public async Task GetByTitleAsync_WhenTestTypeExists_ReturnCorrectTestType()
        {
            // Arrange
            var testType = CreateTestTestType();
            var context = CreateInMemoryDbContext(testType);

            var mockLogger = new Mock<ILogger<TestTypesRepo>>();
            var repo = new TestTypesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByTitleAsync(ValidTestTypeTitle);

            // Assert
            result.Should().BeEquivalentTo(testType);
        }

        [Fact]
        public async Task GetAllTestTypesAsync_WhenTestTypesExist_ReturnListOfTestTypes()
        {
            // Arrange
            var testType = CreateTestTestType();
            var context = CreateInMemoryDbContext(testType);

            var mockLogger = new Mock<ILogger<TestTypesRepo>>();
            var repo = new TestTypesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllTestTypesAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(testType);
        }

        [Fact]
        public async Task AddTestTypeAsync_WhenTestTypeIsValid_ReturnTestTypeId()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestTypesRepo>>();
            var repo = new TestTypesRepo(context, mockLogger.Object);

            var testType = CreateTestTestType();

            // Act
            var result = await repo.AddTestTypeAsync(testType);

            // Assert
            result.Should().Be(ValidTestTypeId);
        }

        [Fact]
        public async Task UpdateTestTypeAsync_WhenTestTypeIsValid_ReturnTrue()
        {
            // Arrange
            var testType = CreateTestTestType();
            var context = CreateInMemoryDbContext(testType);

            var mockLogger = new Mock<ILogger<TestTypesRepo>>();
            var repo = new TestTypesRepo(context, mockLogger.Object);

            // Act
            testType.Title = "Updated Test Type Title";
            var result = await repo.UpdateTestTypeAsync(testType);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region Negative Tests

        [Fact]
        public async Task GetByIdAsync_WhenTestTypeDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestTypesRepo>>();
            var repo = new TestTypesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByIdAsync(1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WhenTestTypeIdIsZeroOrMinus_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestTypesRepo>>();
            var repo = new TestTypesRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetByIdAsync(-1));
        }

        [Fact]
        public async Task GetByTitleAsync_WhenTestTypeDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestTypesRepo>>();
            var repo = new TestTypesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByTitleAsync("NonExistentTestType");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByTitleAsync_WhenTestTypeTitleIsNullOrEmpty_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestTypesRepo>>();
            var repo = new TestTypesRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetByTitleAsync(""));
        }

        [Fact]
        public async Task GetAllTestTypesAsync_WhenNoTestTypesExist_ReturnEmptyList()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestTypesRepo>>();
            var repo = new TestTypesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllTestTypesAsync();

            // Assert
            result.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task AddTestTypeAsync_WhenTestTypeIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestTypesRepo>>();
            var repo = new TestTypesRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.AddTestTypeAsync(null!));
        }

        [Fact]
        public async Task UpdateTestTypeAsync_WhenTestTypeIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<TestTypesRepo>>();
            var repo = new TestTypesRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.UpdateTestTypeAsync(null!));
        }

        #endregion

        #region Special Cases

        [Fact]
        public async Task GetByIdAsync_WhenDbOperationFails_ThrowException()
        {
            // Arrange
            var mockDbContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<TestType>>();

            mockDbSet.Setup(x => x.FindAsync(ValidTestTypeId)).ThrowsAsync(new Exception("Database operation fails"));
            mockDbContext.Setup(x => x.TestTypes).Returns(mockDbSet.Object);

            var mockLogger = new Mock<ILogger<TestTypesRepo>>();
            var repo = new TestTypesRepo(mockDbContext.Object, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await repo.GetByIdAsync(ValidTestTypeId));
        }

        [Fact]
        public async Task AddTestTypeAsync_WhenOperationTimesOut_ThrowTaskCancellationException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                  .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                  .Options;

            var mockDbContext = new Mock<AppDbContext>(options);
            var mockDbSet = new Mock<DbSet<TestType>>();

            mockDbContext.Setup(x => x.TestTypes).Returns(mockDbSet.Object);

            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new TaskCanceledException("Operation Timed Out."));

            var mockLogger = new Mock<ILogger<TestTypesRepo>>();
            var repo = new TestTypesRepo(mockDbContext.Object, mockLogger.Object);

            var testType = CreateTestTestType();

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(async () => await repo.AddTestTypeAsync(testType));
        }

        #endregion

        #region Private Helpers

        private AppDbContext CreateInMemoryDbContext(TestType testType = null!)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test
                .Options;

            var dbContext = new AppDbContext(options);

            if (testType is not null)
            {
                dbContext.TestTypes.Add(testType);
                dbContext.SaveChanges();
            }

            return dbContext;
        }

        private TestType CreateTestTestType() => new TestType
        {
            Id = ValidTestTypeId,
            Title = ValidTestTypeTitle,
            Description = "Test Description",
            Fees = 50.0m
        };

        #endregion
    }
}