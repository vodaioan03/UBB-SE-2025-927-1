using Xunit;
using Microsoft.AspNetCore.Mvc;
using Duo.Api.Controllers;
using Duo.Api.Models;
using Duo.Api.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace DuoTests
{
    public class UserControllerIntegrationTests : IDisposable
    {
        private readonly DataContext context;
        private readonly UserController controller;

        public UserControllerIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new DataContext(options);
            var repository = new Repository(context);
            controller = new UserController(repository);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        [Fact]
        public async Task Register_ValidUser_ReturnsCreated()
        {
            // Arrange
            var newUser = new User
            {
                Username = "testuser",
                Email = "test@example.com"
            };

            // Act
            var result = await controller.Register(newUser) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(newUser.Username, (result.Value as dynamic).Username);
        }

        [Fact]
        public async Task Register_InvalidUser_ReturnsBadRequest()
        {
            // Act & Assert
            var resultNull = await controller.Register(null) as BadRequestObjectResult;
            Assert.NotNull(resultNull);

            var resultEmptyUsername = await controller.Register(new User { Email = "test@example.com" }) as BadRequestObjectResult;
            Assert.NotNull(resultEmptyUsername);

            var resultEmptyEmail = await controller.Register(new User { Username = "testuser" }) as BadRequestObjectResult;
            Assert.NotNull(resultEmptyEmail);
        }

        [Fact]
        public async Task Login_ValidUser_ReturnsUser()
        {
            // Arrange
            var testUser = new User { Email = "test@example.com" };
            await context.Users.AddAsync(testUser);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Login(new LoginRequest { Email = "test@example.com" }) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("test@example.com", (result.Value as User).Email);
        }

        [Fact]
        public async Task Login_InvalidUser_ReturnsUnauthorized()
        {
            // Act
            var result = await controller.Login(new LoginRequest { Email = "nonexistent@example.com" }) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
        }

        [Fact]
        public async Task GetUserById_ExistingUser_ReturnsUser()
        {
            // Arrange
            var testUser = new User { UserId = 1 };
            await context.Users.AddAsync(testUser);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.GetUserById(1) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(1, (result.Value as User).UserId);
        }

        [Fact]
        public async Task GetUserById_NonExistingUser_ReturnsNotFound()
        {
            // Act
            var result = await controller.GetUserById(999) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Update_ValidUser_UpdatesData()
        {
            // Arrange
            var testUser = new User { UserId = 1, Username = "old", Email = "old@example.com" };
            await context.Users.AddAsync(testUser);
            await context.SaveChangesAsync();

            var updatedUser = new User
            {
                UserId = 1,
                Username = "new",
                Email = "new@example.com",
                NumberOfCompletedSections = 2,
                NumberOfCompletedQuizzesInSection = 3
            };

            // Act
            var result = await controller.Update(updatedUser) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);

            var dbUser = await context.Users.FindAsync(1);
            Assert.Equal("new", dbUser.Username);
            Assert.Equal("new@example.com", dbUser.Email);
            Assert.Equal(2, dbUser.NumberOfCompletedSections);
            Assert.Equal(3, dbUser.NumberOfCompletedQuizzesInSection);
        }

        [Fact]
        public async Task Delete_ExistingUser_RemovesUser()
        {
            // Arrange
            var testUser = new User { UserId = 1 };
            await context.Users.AddAsync(testUser);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Delete(1) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
            Assert.Null(await context.Users.FindAsync(1));
        }
    }
}