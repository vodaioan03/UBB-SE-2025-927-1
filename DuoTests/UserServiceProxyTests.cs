//using Moq;
//using Xunit;
//using Duo.Models;
//using Duo.Repositories;
//using System.Threading.Tasks;
//using System;

//namespace DuoTests
//{
//    public class UserServiceTests
//    {
//        private readonly Mock<IUserRepository> mockRepo;
//        private readonly UserService userService;

//        public UserServiceTests()
//        {
//            mockRepo = new Mock<IUserRepository>();
//            userService = new UserService(mockRepo.Object);
//        }

//        [Fact]
//        public async Task GetByIdAsync_ValidId_ReturnsUser()
//        {
//            // Arrange
//            var testUser = new User { UserId = 1 };
//            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(testUser);

//            // Act
//            var result = await userService.GetByIdAsync(1);

//            // Assert
//            Assert.Equal(testUser, result);
//            mockRepo.Verify(r => r.GetByIdAsync(1), Times.Once);
//        }

//        [Fact]
//        public async Task GetByIdAsync_InvalidId_ThrowsException()
//        {
//            // Act & Assert
//            await Assert.ThrowsAsync<ArgumentException>(() => userService.GetByIdAsync(0));
//        }

//        [Fact]
//        public async Task GetByUsernameAsync_ValidUsername_ReturnsUser()
//        {
//            // Arrange
//            var testUser = new User { Username = "testuser" };
//            mockRepo.Setup(r => r.GetByUsernameAsync("testuser")).ReturnsAsync(testUser);

//            // Act
//            var result = await userService.GetByUsernameAsync("testuser");

//            // Assert
//            Assert.Equal(testUser, result);
//            mockRepo.Verify(r => r.GetByUsernameAsync("testuser"), Times.Once);
//        }

//        [Fact]
//        public async Task GetByUsernameAsync_EmptyUsername_ThrowsException()
//        {
//            // Act & Assert
//            await Assert.ThrowsAsync<ArgumentException>(() => userService.GetByUsernameAsync(""));
//        }

//        [Fact]
//        public async Task CreateUserAsync_ValidUser_ReturnsUserId()
//        {
//            // Arrange
//            var testUser = new User { Username = "testuser", Email = "test@example.com" };
//            mockRepo.Setup(r => r.CreateUserAsync(testUser)).ReturnsAsync(1);

//            // Act
//            var result = await userService.CreateUserAsync(testUser);

//            // Assert
//            Assert.Equal(1, result);
//            mockRepo.Verify(r => r.CreateUserAsync(testUser), Times.Once);
//        }

//        [Fact]
//        public async Task CreateUserAsync_InvalidUser_ThrowsException()
//        {
//            // Act & Assert
//            await Assert.ThrowsAsync<ArgumentNullException>(() => userService.CreateUserAsync(null));
//            await Assert.ThrowsAsync<ArgumentException>(() => userService.CreateUserAsync(new User()));
//        }

//        [Fact]
//        public async Task UpdateUserSectionProgressAsync_ValidData_UpdatesProgress()
//        {
//            // Arrange
//            mockRepo.Setup(r => r.UpdateUserProgressAsync(1, 2, 3)).Returns(Task.CompletedTask);

//            // Act
//            await userService.UpdateUserSectionProgressAsync(1, 2, 3);

//            // Assert
//            mockRepo.Verify(r => r.UpdateUserProgressAsync(1, 2, 3), Times.Once);
//        }

//        [Fact]
//        public async Task UpdateUserSectionProgressAsync_InvalidUserId_ThrowsException()
//        {
//            // Act & Assert
//            await Assert.ThrowsAsync<ArgumentException>(() => userService.UpdateUserSectionProgressAsync(0, 1, 1));
//        }

//        [Fact]
//        public async Task IncrementUserProgressAsync_ValidId_IncrementsProgress()
//        {
//            // Arrange
//            var testUser = new User { UserId = 1, NumberOfCompletedQuizzesInSection = 1 };
//            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(testUser);
//            mockRepo.Setup(r => r.UpdateUserAsync(testUser)).Returns(Task.CompletedTask);

//            // Act
//            await userService.IncrementUserProgressAsync(1);

//            // Assert
//            Assert.Equal(2, testUser.NumberOfCompletedQuizzesInSection);
//            mockRepo.Verify(r => r.UpdateUserAsync(testUser), Times.Once);
//        }

//        [Fact]
//        public async Task IncrementUserProgressAsync_InvalidId_ThrowsException()
//        {
//            // Act & Assert
//            await Assert.ThrowsAsync<ArgumentException>(() => userService.IncrementUserProgressAsync(0));
//        }
//    }
//}