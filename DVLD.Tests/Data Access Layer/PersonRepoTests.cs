using Castle.Core.Logging;
using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Enums;
using DVLD_DataAccess.Core.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Moq;
using System.Net;
using System.Numerics;

namespace DVLD.Tests
{
    public class PersonRepoTests
    {
        /* Method naming Conventions
         * [Fact]
         * public void Method_Scenario_Outcome()
         * { 
         *    1. Arrange
         *    2. Act
         *    3. Assert
         * }
        */

        private const int ValidPersonId = 12;
        private const string ValidNationalNo = "N5";

        #region Find Person
        //Get Person By Id
        [Fact]
        public async Task GetPersonByIdAsync_WhenPersonExists_ReturnsCorrectPerson()
        {
            //Arrange
            var person = CreateTestPerson();
            using var context = CreateInMemoryDbContext(person);

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);

            // Act  
            var result = await repo.GetPersonByIdAsync(ValidPersonId);

            // Assert   
            result.Should().BeEquivalentTo(person);
        }

        [Fact]
        public async Task GetPersonByIdAsync_WhenPersonIdIsZero_ThrowArgumentException()
        {
            //Arrange
            var person = CreateTestPerson();
            using var context = CreateInMemoryDbContext(person);

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetPersonByIdAsync(0));
        }

        [Fact]
        public async Task GetPersonByIdAsync_WhenPersonDoesNotExist_ThrowArgumentException()
        {
            //Arrange
            using var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetPersonByIdAsync(-1));
        }

        [Fact]
        public async Task GetPersonByIdAsync_WhenDbOperationFails_ThrowArgumentNullException()
        {
            //Arrange
            var mockDbContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<Person>>();

            mockDbSet.Setup(x => x.FindAsync(It.IsAny<int>())).ThrowsAsync(new ArgumentNullException("Database Error"));
            mockDbContext.Setup(x => x.People).Returns(mockDbSet.Object);

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(mockDbContext.Object, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => repo.GetPersonByIdAsync(ValidPersonId));
        }

        //Get Person By National No
        [Fact]
        public async Task GetPersonByNationalNoAsync_WhenPersonExists_ReturnsCorrectPerson()
        {
            //Arrange
            var person = CreateTestPerson();
            using var context = CreateInMemoryDbContext(person);

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetPersonByNationalNoAsync(ValidNationalNo);

            // Assert
            result.Should().BeEquivalentTo(person);
        }

        [Fact]
        public async Task GetPersonByNationalNoAsync_WhenPersonDoesNotExist_ReturnsNull()
        {
            //Arrange
            using var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetPersonByNationalNoAsync("N99");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetPersonByNationalNoAsync_WhenNationalNoIsNullOrEmpty_ThrowArgumentException()
        {
            //Arrange
            var person = CreateTestPerson();
            using var context = CreateInMemoryDbContext(person);

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => repo.GetPersonByNationalNoAsync(""));
        }
        #endregion


        #region Add New Person
        [Fact]
        public async Task AddNewPersonAsync_WhenPersonIsValid_ReturnsPersonId()
        {
            //Arrange
            using var context = CreateInMemoryDbContext();
            
            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);

            var person = CreateTestPerson();

            // Act
            var result = await repo.AddNewPersonAsync(person);

            // Assert
            result.Should().Be(ValidPersonId);
        }

        [Fact]
        public async Task AddNewPersonAsync_WhenPersonIsNull_ThrowArgumentNullException()
        {
            //Arrange
            using var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => repo.AddNewPersonAsync(null!));
        }
        #endregion


        #region Update Person
        [Fact]
        public async Task UpdatePersonAsync_WhenPersonIsValid_ReturnsTrue()
        {
            //Arrange
            var person = CreateTestPerson();
            using var context = CreateInMemoryDbContext(person);

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);

            person.FirstName = "Jason";

            // Act
            var result = await repo.UpdatePersonAsync(person);

            // Assert
            result.Should().BeTrue();
            person.FirstName.Should().Be("Jason");
        }

        [Fact]
        public async Task UpdatePersonAsync_WhenPersonIsNull_ThrowArgumentNullException()
        {
            //Arrange
            using var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => repo.UpdatePersonAsync(null!));
        }
        #endregion


        #region Get All People
        [Fact]
        public async Task GetAllPeopleAsync_WhenPeopleExist_ReturnsListOfPeople()
        {
            //Arrange
            var person = CreateTestPerson();
            using var context = CreateInMemoryDbContext(person);
            
            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllPeopleAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(person);
        }

        [Fact]
        public async Task GetAllPeopleAsync_WhenLargeDataSet_ReturnsAllPeople()
        {
            //Arrange
            var people = Enumerable.Range(1, 1000).Select(i => new Person()
            {
                Id = i,
                FirstName = $"FirstName{i}",
                LastName = $"LastName{i}",
                NationalNo = $"N{i}",
                DateOfBirth = new DateTime(1990, 1, 1),
                CountryId = 1
            }).ToList();

            using var context = CreateInMemoryDbContext();
            context.People.AddRange(people);
            await context.SaveChangesAsync();

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);

            // Act
            var result = await repo.GetAllPeopleAsync();

            // Assert
            result.Should().HaveCount(1000);
        }

        [Fact]
        public async Task GetAllPeopleAsync_WhenPeopleDoNotExist_ReturnsEmptyList()
        {
            //Arrange
            using var context = CreateInMemoryDbContext();
            
            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);
            
            // Act
            var result = await repo.GetAllPeopleAsync();

            // Assert
            result.Should().BeEmpty();
        }
        #endregion


        #region Delete Person
        [Fact]
        public async Task DeletePersonAsync_WhenValidPersonId_ReturnsTrue()
        {
            //Arrange
            var person = CreateTestPerson();
            using var context = CreateInMemoryDbContext(person);

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);

            // Act
            var result = await repo.DeletePersonAsync(ValidPersonId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeletePersonAsync_WhenPersonIdIsZero_ThrowArgumentException()
        {
            //Arrange
            var person = CreateTestPerson();
            using var context = CreateInMemoryDbContext(person);

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => repo.DeletePersonAsync(0));
        }

        [Fact]
        public async Task DeletePersonAsync_WhenNonValidPersonId_ThrowArgumentException()
        {
            //Arrange
            using var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => repo.DeletePersonAsync(-1));
        }
        #endregion


        #region Does Person Exists
        [Fact]
        public async Task DoesPersonExistAsync_WhenValidPersonId_ReturnsTrue()
        {
            //Arrange
            var person = CreateTestPerson();
            using var context = CreateInMemoryDbContext(person);

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);

            // Act
            var result = await repo.DoesPersonExistAsync(ValidPersonId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DoesPersonExistAsync_WhenNonValidPersonId_ThrowArgumentException()
        {
            //Arrange
            using var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => repo.DoesPersonExistAsync(-1));
        }

        [Fact]
        public async Task DoesPersonExistAsync_WhenValidNationalNo_ReturnsTrue()
        {
            //Arrange
            var person = CreateTestPerson();
            using var context = CreateInMemoryDbContext(person);

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);

            // Act
            var result = await repo.DoesPersonExistAsync(ValidNationalNo);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DoesPersonExistAsync_WhenNonValidNationalNo_ReturnsFalse()
        {
            //Arrange
            using var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);

            // Act
            var result = await repo.DoesPersonExistAsync("ss");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DoesPersonExistAsync_WhenNationalNoIsNullOrWhiteSpace_ThrowArgumentException()
        {
            //Arrange
            using var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => repo.DoesPersonExistAsync(""));
        }
        #endregion


        //Async Exception
        [Fact]
        public async Task AddNewPersonAsync_WhenOperationTimesOut_ThrowsTaskCanceledException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;

            var mockDbContext = new Mock<AppDbContext>(options);

            // Mock DbSet<Person> for AddAsync
            var mockDbSet = new Mock<DbSet<Person>>();
            mockDbContext.Setup(m => m.People).Returns(mockDbSet.Object);

            // Mock SaveChangesAsync to throw TaskCanceledException
            mockDbContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
                        .ThrowsAsync(new TaskCanceledException("Operation Timed Out"));

            var mockLogger = new Mock<ILogger<PersonRepo>>();
            var repo = new PersonRepo(mockDbContext.Object, mockLogger.Object);


            var person = CreateTestPerson();

            // Act
            Func<Person, Task<int>> test = async (e) => await repo.AddNewPersonAsync(person);

            // Assert
            await Assert.ThrowsAsync<TaskCanceledException>(() => test(person));
        }


        #region Private Helpers
        private AppDbContext CreateInMemoryDbContext(Person person = null!)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test
        .Options;

            var dbContext = new AppDbContext(options);

            if (person is not null)
            {
                dbContext.People.Add(person);
                dbContext.SaveChanges();
            }

            return dbContext;
        }

        private Person CreateTestPerson() => new Person
        {
            Id = ValidPersonId,
            FirstName = "John",
            LastName = "Doe",
            NationalNo = ValidNationalNo,
            DateOfBirth = new DateTime(1990, 1, 1),
            CountryId = 1
        };

        #endregion
    }

}