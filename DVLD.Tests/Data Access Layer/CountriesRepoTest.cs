using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;
using DVLD_DataAccess.Core.Repositories;

namespace DVLD.Tests.DataAccess
{
    public class CountriesRepoTests
    {
        private const int ValidCountryId = 1;
        private const string ValidCountryName = "United States";

        #region Positive Tests

        [Fact]
        public async Task GetAllCountries_WhenCountriesExist_ReturnListOfCountries()
        {
            // Arrange
            var countries = new List<Country>
        {
            new Country { Id = 1, CountryName = "United States" },
            new Country { Id = 2, CountryName = "Canada" }
        };

            var context = CreateInMemoryDbContext(countries);
            var mockLogger = new Mock<ILogger<CountriesRepo>>();
            var repo = new CountriesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllCountries();

            // Assert
            result.Should().HaveCount(2);
            result.Should().ContainEquivalentOf(countries[0]);
            result.Should().ContainEquivalentOf(countries[1]);
        }

        [Fact]
        public async Task GetByCountryName_WhenCountryExists_ReturnCorrectCountry()
        {
            // Arrange
            var country = new Country { Id = ValidCountryId, CountryName = ValidCountryName };
            var context = CreateInMemoryDbContext(new List<Country> { country });
            var mockLogger = new Mock<ILogger<CountriesRepo>>();
            var repo = new CountriesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByCountryName(ValidCountryName);

            // Assert
            result.Should().BeEquivalentTo(country);
        }

        [Fact]
        public async Task GetById_WhenCountryExists_ReturnCorrectCountry()
        {
            // Arrange
            var country = new Country { Id = ValidCountryId, CountryName = ValidCountryName };
            var context = CreateInMemoryDbContext(new List<Country> { country });
            var mockLogger = new Mock<ILogger<CountriesRepo>>();
            var repo = new CountriesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetById(ValidCountryId);

            // Assert
            result.Should().BeEquivalentTo(country);
        }

        #endregion

        #region Negative Tests

        [Fact]
        public async Task GetByCountryName_WhenCountryDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<CountriesRepo>>();
            var repo = new CountriesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetByCountryName("NonExistentCountry");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByCountryName_WhenCountryNameIsNullOrWhiteSpace_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<CountriesRepo>>();
            var repo = new CountriesRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetByCountryName(""));
        }

        [Fact]
        public async Task GetById_WhenCountryDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<CountriesRepo>>();
            var repo = new CountriesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetById(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetById_WhenCountryIdIsZeroOrNegative_ThrowArgumentException()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<CountriesRepo>>();
            var repo = new CountriesRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetById(-1));
        }

        [Fact]
        public async Task GetAllCountries_WhenNoCountriesExist_ReturnEmptyList()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<CountriesRepo>>();
            var repo = new CountriesRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllCountries();

            // Assert
            result.Should().BeEmpty();
        }

        #endregion

        #region Special Cases

        [Fact]
        public async Task GetById_WhenDbOperationFails_ThrowException()
        {
            // Arrange
            var mockDbContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<Country>>();

            mockDbSet.Setup(x => x.FindAsync(ValidCountryId)).ThrowsAsync(new Exception("Database operation fails"));
            mockDbContext.Setup(x => x.Countries).Returns(mockDbSet.Object);

            var mockLogger = new Mock<ILogger<CountriesRepo>>();
            var repo = new CountriesRepo(mockDbContext.Object, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await repo.GetById(ValidCountryId));
        }

        #endregion

        #region Private Helpers

        private AppDbContext CreateInMemoryDbContext(List<Country>? countries = null)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test
                .Options;

            var dbContext = new AppDbContext(options);

            if (countries != null)
            {
                dbContext.Countries.AddRange(countries);
                dbContext.SaveChanges();
            }

            return dbContext;
        }

        #endregion
    }
}