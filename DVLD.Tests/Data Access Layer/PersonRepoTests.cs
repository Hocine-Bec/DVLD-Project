using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Enums;
using DVLD_DataAccess.Core.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
            var repo = new PersonRepo(context);

            // Act
            var result = await repo.GetPersonByIdAsync(ValidPersonId);

            // Assert   
            result.Should().BeEquivalentTo(person);
        }
        
        [Fact]
        public async Task GetPersonByIdAsync_WhenPersonIdIsZero_ReturnsNull()
        {
            //Arrange
            var person = CreateTestPerson();
            using var context = CreateInMemoryDbContext(person);
            var repo = new PersonRepo(context);

            // Act
            var result = await repo.GetPersonByIdAsync(0);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetPersonByIdAsync_WhenPersonDoesNotExist_ReturnsNull()
        {
            //Arrange
            using var context = CreateInMemoryDbContext();
            var repo = new PersonRepo(context);

            // Act
            var result = await repo.GetPersonByIdAsync(-1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetPersonByIdAsync_WhenDbOperationFails_ReturnsNull()
        {
            //Arrange
            var mockDbContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<Person>>();

            mockDbSet.Setup(x => x.FindAsync(It.IsAny<int>())).ThrowsAsync(new Exception("Database Error"));
            mockDbContext.Setup(x => x.People).Returns(mockDbSet.Object);

            var repo = new PersonRepo(mockDbContext.Object);

            //Act
            var result = await repo.GetPersonByIdAsync(ValidPersonId);

            //Assert
            result.Should().BeNull();
        }

        //Get Person By National No
        [Fact]
        public async Task GetPersonByNationalNoAsync_WhenPersonExists_ReturnsCorrectPerson()
        {
            //Arrange
            var person = CreateTestPerson();
            using var context = CreateInMemoryDbContext(person);
            var repo = new PersonRepo(context);

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
            var repo = new PersonRepo(context);

            // Act
            var result = await repo.GetPersonByNationalNoAsync("N99");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetPersonByNationalNoAsync_WhenNationalNoIsNullOrWhiteSpace_ReturnsNull()
        {
            //Arrange
            var person = CreateTestPerson();
            using var context = CreateInMemoryDbContext(person);
            var repo = new PersonRepo(context);

            // Act
            var result = await repo.GetPersonByNationalNoAsync("");

            // Assert
            result.Should().BeNull();
        }
        #endregion


        #region Add New Person
        [Fact]
        public async Task AddNewPersonAsync_WhenPersonIsValid_ReturnsPersonId()
        {
            //Arrange
            using var context = CreateInMemoryDbContext();
            var repo = new PersonRepo(context);
            var person = CreateTestPerson();
            
            // Act
            var result = await repo.AddNewPersonAsync(person);

            // Assert
            result.Should().Be(ValidPersonId);
        }

        [Fact]
        public async Task AddNewPersonAsync_WhenPersonIsNull_ReturnsMinusOne()
        {
            //Arrange
            using var context = CreateInMemoryDbContext();
            var repo = new PersonRepo(context);

            // Act
            var result = await repo.AddNewPersonAsync(null!);

            // Assert
            result.Should().Be(-1);
        }
        #endregion


        #region Update Person
        [Fact]
        public async Task UpdatePersonAsync_WhenPersonIsValid_ReturnsTrue()
        {
            //Arrange
            var person = CreateTestPerson();
            using var context = CreateInMemoryDbContext(person);
            var repo = new PersonRepo(context);
            person.FirstName = "Jason";

            // Act
            var result = await repo.UpdatePersonAsync(person);

            // Assert
            result.Should().BeTrue();
            person.FirstName.Should().Be("Jason");
        }

        [Fact]
        public async Task UpdatePersonAsync_WhenPersonIsNull_ReturnsFalse()
        {
            //Arrange
            using var context = CreateInMemoryDbContext();
            var repo = new PersonRepo(context);

            // Act
            var result = await repo.UpdatePersonAsync(null!);

            // Assert
            result.Should().BeFalse();
        }
        #endregion


        #region Get All People
        [Fact]
        public async Task GetAllPeopleAsync_WhenPeopleExist_ReturnsListOfPeople()
        {
            //Arrange
            var person = CreateTestPerson();
            using var context = CreateInMemoryDbContext(person);            
            var repo = new PersonRepo(context);

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

            var repo = new PersonRepo(context);

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
            var repo = new PersonRepo(context);

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
            var repo = new PersonRepo(context);

            // Act
            var result = await repo.DeletePersonAsync(ValidPersonId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeletePersonAsync_WhenPersonIdIsZero_ReturnsFalse()
        {
            //Arrange
            var person = CreateTestPerson();
            using var context = CreateInMemoryDbContext(person);
            var repo = new PersonRepo(context);

            // Act
            var result = await repo.DeletePersonAsync(0);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeletePersonAsync_WhenNonValidPersonId_ReturnsFalse()
        {
            //Arrange
            using var context = CreateInMemoryDbContext();
            var repo = new PersonRepo(context);

            // Act
            var result = await repo.DeletePersonAsync(-1);

            // Assert
            result.Should().BeFalse();
        }
        #endregion


        #region Does Person Exists
        [Fact]
        public async Task DoesPersonExistAsync_WhenValidPersonId_ReturnsTrue()
        {
            //Arrange
            var person = CreateTestPerson();
            using var context = CreateInMemoryDbContext(person);
            var repo = new PersonRepo(context);

            // Act
            var result = await repo.DoesPersonExistAsync(ValidPersonId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DoesPersonExistAsync_WhenNonValidPersonId_ReturnsFalse()
        {
            //Arrange
            using var context = CreateInMemoryDbContext();
            var repo = new PersonRepo(context);

            // Act
            var result = await repo.DoesPersonExistAsync(-1);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DoesPersonExistAsync_WhenValidNationalNo_ReturnsTrue()
        {
            //Arrange
            var person = CreateTestPerson();
            using var context = CreateInMemoryDbContext(person);
            var repo = new PersonRepo(context);

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
            var repo = new PersonRepo(context);

            // Act
            var result = await repo.DoesPersonExistAsync("ss");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DoesPersonExistAsync_WhenNationalNoIsNullOrWhiteSpace_ReturnsFalse()
        {
            //Arrange
            using var context = CreateInMemoryDbContext();
            var repo = new PersonRepo(context);

            // Act
            var result = await repo.DoesPersonExistAsync("");

            // Assert
            result.Should().BeFalse();
        }
        #endregion


        //Async Exception
        [Fact]
        public async Task AddNewPersonAsync_WhenOperationTimesOut_ThrowsException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: new Guid().ToString()).Options;

            var mockDbContext = new Mock<AppDbContext>(options);
            mockDbContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new TaskCanceledException("Operation Timed Out"));

            var repo = new PersonRepo(mockDbContext.Object);
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