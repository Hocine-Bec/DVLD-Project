using Castle.Core.Logging;
using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Tests
{
    public class UserRepoTests
    {
        private const int ValidPersonId = 5;
        private const int ValidUserId = 10;


        #region Positive Tests
        [Fact]
        public async Task GetUserInfoByUserIdAsync_WhenUserExists_ReturnCorrectUser()
        {
            //Arrange
            var user = CreateTestUser();
            var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act
            var result = await repo.GetByUserIdAsync(ValidUserId);

            //Assert
            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task GetByPersonIdAsync_WhenUserExistsWithPersonId_ReturnCorrectUser()
        {
            //Arrange
            var user = CreateTestUser();
            var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act
            var result = await repo.GetByPersonIdAsync(ValidPersonId);

            //Assert
            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task GetByUsernameAndPasswordAsync_WhenUserExistsWithUsernameAndPassword_ReturnCorrectUser()
        {
            //Arrange
            var user = CreateTestUser();
            var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act
            var result = await repo.GetByUsernameAndPasswordAsync(user.Username, user.Password);

            //Assert
            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task AddNewUserAsync_WhenUserIsValid_ReturnUserId()
        {
            //Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            var user = CreateTestUser();

            //Act
            var result = await repo.AddNewUserAsync(user);

            //Assert
            result.Should().Be(ValidUserId);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenUserIsValid_ReturnTrue()
        {
            //Arrange
            var user = CreateTestUser();
            var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act
            user.Password = "11";
            var result = await repo.UpdateUserAsync(user);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetAllUsersAsync_WhenUsersExists_ReturnListOfUsers()
        {
            //Arrange
            var user = CreateTestUser();
            var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act
            var result = await repo.GetAllUsersAsync();

            //Assert
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(user);
        }

        [Fact]
        public async Task GetAllUsersAsync_WhenLargeDataSet_ReturnAllUsers()
        {
            //Arrange
            var users = Enumerable.Range(1, 1000).Select(i => new User()
            {
                Id = i,
                PersonId = i + 2,
                Username = $"Eddard {i}",
                Password = "1234",
                IsActive = true
            }).ToList();

            var context = CreateInMemoryDbContext();
            context.Users.AddRange(users);
            await context.SaveChangesAsync();

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act
            var result = await repo.GetAllUsersAsync();

            //Assert
            result.Should().HaveCount(1000);
        }

        [Fact]
        public async Task DoesUserExistAsync_WhenValidUserId_ReturnTrue()
        {
            //Arrange
            var user = CreateTestUser();
            var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act
            var result = await repo.DoesUserExistAsync(ValidUserId);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DoesUserExistAsync_WhenValidUsername_ReturnTrue()
        {
            //Arrange
            var user = CreateTestUser();
            var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act
            var result = await repo.DoesUserExistAsync(user.Username);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserAsync_WhenUsersExists_ReturnTrue()
        {
            //Arrange
            var user = CreateTestUser();
            var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act
            var result = await repo.DeleteUserAsync(ValidUserId);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DoesPersonHaveUserAsync_WhenValidPersonId_ReturnTrue()
        {
            //Arrange
            var user = CreateTestUser();
            using var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            // Act
            var result = await repo.DoesPersonHaveUserAsync(ValidPersonId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenValidUserId_ReturnTrue()
        {
            //Arrange
            var user = CreateTestUser();
            using var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            // Act
            var result = await repo.ChangePasswordAsync(ValidUserId, "11");

            // Assert
            result.Should().BeTrue();
            user.Password.Should().Be("11");
        }
        #endregion

        #region Negative Tests

        // Find By UserId
        [Fact]
        public async Task GetByUserIdAsync_WhenUserDoesNotExists_ReturnNull()
        {
            //Arrange
            var user = CreateTestUser();
            var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act
            var result = await repo.GetByUserIdAsync(1);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByUserIdAsync_WhenUserIdIsZeroOrMinus_ThrowArgumentException()
        {
            //Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetByUserIdAsync(-11));
        }
       
        //Find By Person Id
        [Fact]
        public async Task GetByPersonIdAsync_WhenPersonIdIsZeroOrMinus_ThrowArgumentException()
        {
            //Arrange
            var user = CreateTestUser();
            var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetByPersonIdAsync(-11));
        }

        [Fact]
        public async Task GetByPersonIdAsync_WhenUserDoesNotExistsWithPersonId_ReturnNull()
        {
            //Arrange
            var user = CreateTestUser();
            var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act
            var result = await repo.GetByPersonIdAsync(1);

            //Assert
            result.Should().BeNull();
        }


        //Find By Username And Password
        [Fact]
        public async Task GetByUsernameAndPasswordAsync_WhenUserDoesNotExistsWithUsernameAndPassword_ReturnNull()
        {
            //Arrange
            var user = CreateTestUser();
            var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act
            var result = await repo.GetByUsernameAndPasswordAsync("test1", "test2");

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByUsernameAndPasswordAsync_WhenPasswordIsWrong_ReturnNull()
        {
            //Arrange
            var user = CreateTestUser();
            var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act
            var result = await repo.GetByUsernameAndPasswordAsync(user.Username, "test2");

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByUsernameAndPasswordAsync_WhenUsernameIsWrong_ReturnNull()
        {
            //Arrange
            var user = CreateTestUser();
            var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act
            var result = await repo.GetByUsernameAndPasswordAsync("test1", user.Password);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByUsernameAndPasswordAsync_WhenInvalidUsername_ThrowArgumentException()
        {
            //Arrange
            var user = CreateTestUser();
            var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => 
                                await repo.GetByUsernameAndPasswordAsync("", user.Password));
        }

        [Fact]
        public async Task GetByUsernameAndPasswordAsync_WhenInvalidPassword_ThrowArgumentException()
        {
            //Arrange
            var user = CreateTestUser();
            var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                                await repo.GetByUsernameAndPasswordAsync(user.Username, ""));
        }


        // Add New User
        [Fact]
        public async Task AddNewUserAsync_WhenUserIsNull_ThrowArgumentNullException()
        {
            //Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.AddNewUserAsync(null!));
        }

        // Update User
        [Fact]
        public async Task UpdateUserAsync_WhenUserIsNull_ThrowArgumentNullException()
        {
            //Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.UpdateUserAsync(null!));
        }

        // Get All Users
        [Fact]
        public async Task GetAllUsersAsync_WhenUsersDoesNotExists_ReturnEmptyList()
        {
            //Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act
            var result = await repo.GetAllUsersAsync();

            //Assert
            result.Should().BeNullOrEmpty();
        }

        // Delete User
        [Fact]
        public async Task DeleteUserAsync_WhenUsersDoesNotExists_ThrowArgumentNullException()
        {
            //Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.DeleteUserAsync(1));
        }

        [Fact]
        public async Task DeleteUserAsync_WhenUserIdIsZeroOrMinus_ThrowArgumentException()
        {
            //Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.DeleteUserAsync(-1));
        }

        // Does User Exists By User Id
        [Fact]
        public async Task DoesUserExistAsync_WhenNonValidUserId_ThrowArgumentException()
        {
            //Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.DoesUserExistAsync(-1));
        }

        // Does User Exists By Username
        [Fact]
        public async Task DoesUserExistAsync_WhenNonValidUsername_ThrowArgumentException()
        {
            //Arrange
            var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.DoesUserExistAsync(""));
        }

        // Is Person A User? By Person Id
        [Fact]
        public async Task DoesPersonHaveUserAsync_WhenNonValidPersonId_ThrowArgumentException()
        {
            //Arrange
            using var context = CreateInMemoryDbContext();

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => repo.DoesPersonHaveUserAsync(-1));
        }

        //Change Password
        [Fact]
        public async Task ChangePasswordAsync_WhenInValidUserId_ThrowArgumentException()
        {
            //Arrange
            var user = CreateTestUser();
            using var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await repo.ChangePasswordAsync(-1, "11"));
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenUserDoesNotExist_ThrowArgumentNullException()
        {
            //Arrange
            var user = CreateTestUser();
            using var context = CreateInMemoryDbContext(user);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(context, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.ChangePasswordAsync(1, "11"));
        }
        #endregion

        #region Special Case
        [Fact]
        public async Task GetByUserIdAsync_WhenDbOperationFails_ThrowException()
        {
            //Arrange
            var mockDbContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.Setup(x => x.FindAsync(ValidUserId)).ThrowsAsync(new Exception("Database operation fails"));
            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(mockDbContext.Object, mockLogger.Object);

            //Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await repo.GetByUserIdAsync(ValidUserId));
        }

        [Fact]
        public async Task AddNewUserAsync_WhenOperationTimesOut_ThrowTaskCancellationException()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                  .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                  .Options;

            var mockDbContext = new Mock<AppDbContext>(options);
            var mockDbSet = new Mock<DbSet<User>>();

            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new TaskCanceledException("Operation Timed Out."));

            var mockLogger = new Mock<ILogger<UserRepo>>();
            var repo = new UserRepo(mockDbContext.Object, mockLogger.Object);

            var user = CreateTestUser();

            //Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(async () => await repo.AddNewUserAsync(user));
        }
        #endregion

        #region Private Helpers
        private AppDbContext CreateInMemoryDbContext(User user = null!)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test
        .Options;

            var dbContext = new AppDbContext(options);

            if (user is not null)
            {
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
            }

            return dbContext;
        }

        private User CreateTestUser() => new User
        {
            Id = ValidUserId,
            PersonId = ValidPersonId,
            Username = "Eddard",
            Password = "1234",
            IsActive = true
        };
        #endregion
    }
}
