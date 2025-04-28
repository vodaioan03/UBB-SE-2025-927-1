using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Api.Persistence;
using Duo.Api.Repositories;
using Duo.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Quizzes;
using Duo.Api.Models.Sections;

namespace Duo.Api.Tests.Repositories
{
    [TestClass]
    public class RepositoryTests
    {
        private DataContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;

            return new DataContext(options);
        }

        #region User
        [TestMethod]
        public async Task GetUsersFromDbAsync_ReturnsAllUsers()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Users.Add(new User { UserId = 1, Username = "Alice" });
            context.Users.Add(new User { UserId = 2, Username = "Bob" });
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var users = await repository.GetUsersFromDbAsync();

            // Assert
            Assert.AreEqual(2, users.Count);
        }

        [TestMethod]
        public async Task AddUserAsync_AddsUserSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new Repository(context);

            var user = new User
            {
                UserId = 3,
                Username = "Charlie",
                Email = "charlie@example.com",
                CoinBalance = 100,
                LastLoginTime = DateTime.UtcNow
            };

            // Act
            await repository.AddUserAsync(user);

            // Assert
            var savedUser = await context.Users.FindAsync(3);
            Assert.IsNotNull(savedUser);
            Assert.AreEqual("Charlie", savedUser.Username);
            Assert.AreEqual("charlie@example.com", savedUser.Email);
            Assert.AreEqual(100, savedUser.CoinBalance);
        }

        [TestMethod]
        public async Task DeleteUserAsync_RemovesUserSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User { UserId = 4, Username = "Dave" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.DeleteUserAsync(4);

            // Assert
            var deletedUser = await context.Users.FindAsync(4);
            Assert.IsNull(deletedUser);
        }

        [TestMethod]
        public async Task UpdateUserAsync_UpdatesUserSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User
            {
                UserId = 5,
                Username = "Eve",
                Email = "eve@original.com",
                CoinBalance = 50,
                LastLoginTime = DateTime.UtcNow
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            user.Username = "Updated Eve";
            user.Email = "eve@updated.com";
            user.CoinBalance = 200;
            await repository.UpdateUserAsync(user);

            // Assert
            var updatedUser = await context.Users.FindAsync(5);
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual("Updated Eve", updatedUser.Username);
            Assert.AreEqual("eve@updated.com", updatedUser.Email);
            Assert.AreEqual(200, updatedUser.CoinBalance);
        }
        #endregion

        #region Tag
        [TestMethod]
        public async Task GetTagsFromDbAsync_ReturnsAllTags()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Tags.Add(new Tag { TagId = 1, Name = "Math" });
            context.Tags.Add(new Tag { TagId = 2, Name = "Science" });
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var tags = await repository.GetTagsFromDbAsync();

            // Assert
            Assert.AreEqual(2, tags.Count);
        }

        [TestMethod]
        public async Task AddTagAsync_AddsTagSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new Repository(context);

            var tag = new Tag
            {
                TagId = 3,
                Name = "History"
            };

            await repository.AddTagAsync(tag);

            // Act
            var savedTag = await context.Tags.FindAsync(3);

            // Assert
            Assert.IsNotNull(savedTag);
            Assert.AreEqual("History", savedTag.Name);
        }

        [TestMethod]
        public async Task DeleteTagAsync_RemovesTagSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var tag = new Tag { TagId = 4, Name = "Geography" };
            context.Tags.Add(tag);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.DeleteTagAsync(4);
            var deletedTag = await context.Tags.FindAsync(4);

            // Assert
            Assert.IsNull(deletedTag);
        }

        [TestMethod]
        public async Task UpdateTagAsync_UpdatesTagSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var tag = new Tag { TagId = 5, Name = "Art" };
            context.Tags.Add(tag);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            tag.Name = "Fine Art";
            await repository.UpdateTagAsync(tag);
            var updatedTag = await context.Tags.FindAsync(5);

            // Assert
            Assert.IsNotNull(updatedTag);
            Assert.AreEqual("Fine Art", updatedTag.Name);
        }

        [TestMethod]
        public async Task GetTagByIdAsync_ReturnsCorrectTag()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var tag = new Tag { TagId = 6, Name = "Programming" };
            context.Tags.Add(tag);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var foundTag = await repository.GetTagByIdAsync(6);

            // Assert
            Assert.IsNotNull(foundTag);
            Assert.AreEqual("Programming", foundTag.Name);
        }
        #endregion

        #region Module
        [TestMethod]
        public async Task GetModulesFromDbAsync_ReturnsAllModules()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Modules.Add(new Module { ModuleId = 1, Title = "Module 1", Description = "Desc 1", ImageUrl = "img1.png" });
            context.Modules.Add(new Module { ModuleId = 2, Title = "Module 2", Description = "Desc 2", ImageUrl = "img2.png" });
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var modules = await repository.GetModulesFromDbAsync();

            // Assert
            Assert.AreEqual(2, modules.Count);
        }

        [TestMethod]
        public async Task GetModuleByIdAsync_ReturnsCorrectModule()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var module = new Module { ModuleId = 3, Title = "Module 3", Description = "Desc 3", ImageUrl = "img3.png" };
            context.Modules.Add(module);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var foundModule = await repository.GetModuleByIdAsync(3);

            // Assert
            Assert.IsNotNull(foundModule);
            Assert.AreEqual("Module 3", foundModule.Title);
        }

        [TestMethod]
        public async Task AddModuleAsync_AddsModuleSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new Repository(context);

            var module = new Module
            {
                ModuleId = 4,
                Title = "Module 4",
                Description = "Desc 4",
                ImageUrl = "img4.png"
            };

            await repository.AddModuleAsync(module);

            // Act
            var savedModule = await context.Modules.FindAsync(4);

            // Assert
            Assert.IsNotNull(savedModule);
            Assert.AreEqual("Module 4", savedModule.Title);
        }

        [TestMethod]
        public async Task UpdateModuleAsync_UpdatesModuleSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var module = new Module
            {
                ModuleId = 5,
                Title = "Old Title",
                Description = "Old Desc",
                ImageUrl = "old.png"
            };
            context.Modules.Add(module);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            module.Title = "New Title";
            module.Description = "New Desc";
            await repository.UpdateModuleAsync(module);

            var updatedModule = await context.Modules.FindAsync(5);

            // Assert
            Assert.IsNotNull(updatedModule);
            Assert.AreEqual("New Title", updatedModule.Title);
            Assert.AreEqual("New Desc", updatedModule.Description);
        }

        [TestMethod]
        public async Task DeleteModuleAsync_RemovesModuleSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var module = new Module { ModuleId = 6, Title = "Module 6", Description = "Desc 6", ImageUrl = "img6.png" };
            context.Modules.Add(module);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.DeleteModuleAsync(6);

            var deletedModule = await context.Modules.FindAsync(6);

            // Assert
            Assert.IsNull(deletedModule);
        }

        [TestMethod]
        public async Task OpenModuleAsync_UpdatesModuleSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var module = new Module
            {
                ModuleId = 7,
                Title = "Module 7",
                Description = "Desc 7",
                ImageUrl = "img7.png"
            };
            context.Modules.Add(module);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.OpenModuleAsync(userId: 1, moduleId: 7);

            var openedModule = await context.Modules.FindAsync(7);

            // Assert
            Assert.IsNotNull(openedModule);
            Assert.AreEqual("Module 7", openedModule.Title);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Module not found")]
        public async Task OpenModuleAsync_ThrowsException_WhenModuleNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new Repository(context);

            // Act
            await repository.OpenModuleAsync(userId: 1, moduleId: 999);
        }
        #endregion

        #region Exercise
        [TestMethod]
        public async Task GetExercisesFromDbAsync_ReturnsAllExercises()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Exercises.Add(new AssociationExercise(1, "Match the following countries with their capitals", Difficulty.Normal,
                new List<string> { "USA", "Canada", "Mexico" }, new List<string> { "Washington D.C.", "Ottawa", "Mexico City" }));
            context.Exercises.Add(new AssociationExercise(2, "Match the colors with their hex codes", Difficulty.Easy,
                new List<string> { "Red", "Green", "Blue" }, new List<string> { "#FF0000", "#00FF00", "#0000FF" }));
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var exercises = await repository.GetExercisesFromDbAsync();

            // Assert
            Assert.AreEqual(2, exercises.Count);
        }

        [TestMethod]
        public async Task GetExerciseByIdAsync_ReturnsCorrectExercise()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var exercise = new AssociationExercise(3, "Match fruits with their colors", Difficulty.Normal,
                new List<string> { "Apple", "Banana", "Grapes" }, new List<string> { "Red", "Yellow", "Purple" });
            context.Exercises.Add(exercise);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var foundExercise = await repository.GetExerciseByIdAsync(3);

            // Assert
            Assert.IsNotNull(foundExercise);
            Assert.AreEqual("Match fruits with their colors", foundExercise.Question);
        }

        [TestMethod]
        public async Task AddExerciseAsync_AddsExerciseSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new Repository(context);

            var exercise = new AssociationExercise(
                4,
                "Match animals with their sounds",
                Difficulty.Hard,
                new List<string> { "Dog", "Cat", "Cow" },
                new List<string> { "Bark", "Meow", "Moo" }
            );

            await repository.AddExerciseAsync(exercise);

            // Act
            var savedExercise = await context.Exercises.FindAsync(4);

            // Assert
            Assert.IsNotNull(savedExercise);
            Assert.AreEqual("Match animals with their sounds", savedExercise.Question);
        }

        [TestMethod]
        public async Task UpdateExerciseAsync_UpdatesExerciseSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var exercise = new AssociationExercise(
                5,
                "Match countries with their capitals",
                Difficulty.Normal,
                new List<string> { "France", "Germany", "Italy" },
                new List<string> { "Paris", "Berlin", "Rome" }
            );
            context.Exercises.Add(exercise);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            exercise.Question = "Match European countries with their capitals";
            await repository.UpdateExerciseAsync(exercise);

            // Act
            var updatedExercise = await context.Exercises.FindAsync(5);

            // Assert
            Assert.IsNotNull(updatedExercise);
            Assert.AreEqual("Match European countries with their capitals", updatedExercise.Question);
        }

        [TestMethod]
        public async Task DeleteExerciseAsync_RemovesExerciseSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var exercise = new AssociationExercise(6, "Match planets with their moons", Difficulty.Easy,
                new List<string> { "Earth", "Mars", "Jupiter" }, new List<string> { "Moon", "Phobos", "Io" });
            context.Exercises.Add(exercise);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            await repository.DeleteExerciseAsync(6);

            // Act
            var deletedExercise = await context.Exercises.FindAsync(6);

            // Assert
            Assert.IsNull(deletedExercise);
        }
        #endregion

        #region Quiz
        [TestMethod]
        public async Task GetQuizzesFromDbAsync_ReturnsAllQuizzes()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var section = new Section { Id = 1, Title = "Section 1", Description = "Description of Section 1" };
            context.Sections.Add(section);

            var quiz1 = new Quiz { Id = 1, SectionId = section.Id, OrderNumber = 1 };
            var quiz2 = new Quiz { Id = 2, SectionId = section.Id, OrderNumber = 2 };

            context.Quizzes.Add(quiz1);
            context.Quizzes.Add(quiz2);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var quizzes = await repository.GetQuizzesFromDbAsync();

            // Assert
            Assert.AreEqual(2, quizzes.Count);
        }


        [TestMethod]
        public async Task GetQuizByIdAsync_ReturnsCorrectQuiz()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var section = new Section { Id = 1, Title = "Section 1", Description = "Description of Section 1" };
            context.Sections.Add(section);

            var quiz = new Quiz { Id = 1, SectionId = section.Id, OrderNumber = 1 };
            context.Quizzes.Add(quiz);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetQuizByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Section 1", result.Section?.Title);
        }


        [TestMethod]
        public async Task AddQuizAsync_AddsQuizToDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var section = new Section { Id = 1, Title = "Section 1", Description = "Description of Section 1" };
            context.Sections.Add(section);

            var quiz = new Quiz { Id = 1, SectionId = section.Id, OrderNumber = 1 };
            var repository = new Repository(context);

            // Act
            await repository.AddQuizAsync(quiz);

            // Assert
            var addedQuiz = await context.Quizzes.FindAsync(1);
            Assert.IsNotNull(addedQuiz);
            Assert.AreEqual(1, addedQuiz?.Id);
            Assert.AreEqual("Section 1", addedQuiz?.Section?.Title);
        }


        [TestMethod]
        public async Task UpdateQuizAsync_UpdatesQuizInDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var section = new Section { Id = 1, Title = "Section 1", Description = "Description of Section 1" };
            context.Sections.Add(section);

            var quiz = new Quiz { Id = 1, SectionId = section.Id, OrderNumber = 1 };
            context.Quizzes.Add(quiz);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            quiz.OrderNumber = 2;
            await repository.UpdateQuizAsync(quiz);

            // Assert
            var updatedQuiz = await context.Quizzes.FindAsync(1);
            Assert.AreEqual(2, updatedQuiz?.OrderNumber);
        }


        [TestMethod]
        public async Task DeleteQuizAsync_DeletesQuizFromDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var section = new Section { Id = 1, Title = "Section 1", Description = "Description of Section 1" };
            context.Sections.Add(section);

            var quiz = new Quiz { Id = 1, SectionId = section.Id, OrderNumber = 1 };
            context.Quizzes.Add(quiz);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.DeleteQuizAsync(1);

            // Assert
            var deletedQuiz = await context.Quizzes.FindAsync(1);
            Assert.IsNull(deletedQuiz);
        }
        #endregion

        #region Course
        [TestMethod]
        public async Task GetCoursesFromDbAsync_ReturnsAllCourses()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var course1 = new Course
            {
                CourseId = 1,
                Title = "Course 1",
                Description = "Description of Course 1",
                IsPremium = false,
                Cost = 100,
                ImageUrl = "http://example.com/course1.jpg",
                TimeToComplete = 3600,
                Difficulty = "Beginner"
            };
            var course2 = new Course
            {
                CourseId = 2,
                Title = "Course 2",
                Description = "Description of Course 2",
                IsPremium = true,
                Cost = 200,
                ImageUrl = "http://example.com/course2.jpg",
                TimeToComplete = 7200,
                Difficulty = "Intermediate"
            };
            context.Courses.Add(course1);
            context.Courses.Add(course2);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var courses = await repository.GetCoursesFromDbAsync();

            // Assert
            Assert.AreEqual(2, courses.Count);
        }

        [TestMethod]
        public async Task GetCourseByIdAsync_ReturnsCorrectCourse()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var course = new Course
            {
                CourseId = 1,
                Title = "Course 1",
                Description = "Description of Course 1",
                IsPremium = false,
                Cost = 100,
                ImageUrl = "http://example.com/course1.jpg",
                TimeToComplete = 3600,
                Difficulty = "Beginner"
            };
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetCourseByIdAsync(1);

            // Assert
            Assert.IsNotNull(result); 
            Assert.AreEqual(1, result.CourseId);
            Assert.AreEqual("Course 1", result.Title); 
        }

        [TestMethod]
        public async Task AddCourseAsync_AddsCourseToDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var course = new Course
            {
                CourseId = 1,
                Title = "New Course",
                Description = "New Course Description",
                IsPremium = true,
                Cost = 250,
                ImageUrl = "http://example.com/newcourse.jpg",
                TimeToComplete = 5000,
                Difficulty = "Advanced"
            };
            var repository = new Repository(context);

            // Act
            await repository.AddCourseAsync(course);

            // Assert
            var addedCourse = await context.Courses.FindAsync(1);
            Assert.IsNotNull(addedCourse);
            Assert.AreEqual(1, addedCourse?.CourseId);
            Assert.AreEqual("New Course", addedCourse?.Title);
        }

        [TestMethod]
        public async Task UpdateCourseAsync_UpdatesCourseInDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var course = new Course
            {
                CourseId = 1,
                Title = "Course 1",
                Description = "Description of Course 1",
                IsPremium = false,
                Cost = 100,
                ImageUrl = "http://example.com/course1.jpg",
                TimeToComplete = 3600,
                Difficulty = "Beginner"
            };
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            course.Title = "Updated Course Title"; 
            await repository.UpdateCourseAsync(course);

            // Assert
            var updatedCourse = await context.Courses.FindAsync(1); 
            Assert.AreEqual("Updated Course Title", updatedCourse?.Title);
        }

        [TestMethod]
        public async Task DeleteCourseAsync_DeletesCourseFromDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var course = new Course
            {
                CourseId = 1,
                Title = "Course 1",
                Description = "Description of Course 1",
                IsPremium = false,
                Cost = 100,
                ImageUrl = "http://example.com/course1.jpg",
                TimeToComplete = 3600,
                Difficulty = "Beginner"
            };
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.DeleteCourseAsync(1);

            // Assert
            var deletedCourse = await context.Courses.FindAsync(1);
            Assert.IsNull(deletedCourse);
        }
        #endregion

        #region CourseCompletion
        [TestMethod]
        public async Task EnrollUserInCourseAsync_EnrollsUserIfNotAlreadyEnrolled()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User { UserId = 1, Username = "Test User", CoinBalance = 100, LastLoginTime = DateTime.Now };
            var course = new Course
            {
                CourseId = 1,
                Title = "Test Course",
                Description = "Test Course Description",
                IsPremium = false,
                Cost = 100,
                ImageUrl = "http://example.com/course.jpg",
                TimeToComplete = 3600,
                Difficulty = "Beginner"
            };
            context.Users.Add(user);
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.EnrollUserInCourseAsync(user.UserId, course.CourseId);

            // Assert
            var courseCompletion = await context.CourseCompletions
                .FirstOrDefaultAsync(cc => cc.UserId == user.UserId && cc.CourseId == course.CourseId);
            Assert.IsNotNull(courseCompletion);
            Assert.AreEqual(false, courseCompletion.CompletionRewardClaimed);
        }

        [TestMethod]
        public async Task IsUserEnrolledInCourseAsync_ReturnsTrueIfUserIsEnrolled()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User { UserId = 1, Username = "Test User", CoinBalance = 100, LastLoginTime = DateTime.Now };
            var course = new Course
            {
                CourseId = 1,
                Title = "Test Course",
                Description = "Test Course Description",
                IsPremium = false,
                Cost = 100,
                ImageUrl = "http://example.com/course.jpg",
                TimeToComplete = 3600,
                Difficulty = "Beginner"
            };
            context.Users.Add(user);
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            var repository = new Repository(context); 
            await repository.EnrollUserInCourseAsync(user.UserId, course.CourseId); 

            // Act
            var result = await repository.IsUserEnrolledInCourseAsync(user.UserId, course.CourseId);

            // Assert
            Assert.IsTrue(result); 
        }

        [TestMethod]
        public async Task IsCourseCompletedAsync_ReturnsTrueIfCourseIsCompleted()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User { UserId = 1, Username = "Test User", CoinBalance = 100, LastLoginTime = DateTime.Now };
            var course = new Course
            {
                CourseId = 1,
                Title = "Test Course",
                Description = "Test Course Description",
                IsPremium = false,
                Cost = 100,
                ImageUrl = "http://example.com/course.jpg",
                TimeToComplete = 3600,
                Difficulty = "Beginner"
            };
            var courseCompletion = new CourseCompletion
            {
                UserId = user.UserId,
                CourseId = course.CourseId,
                CompletionRewardClaimed = false,
                TimedRewardClaimed = false,
                CompletedAt = DateTime.Now
            };
            context.Users.Add(user);
            context.Courses.Add(course);
            context.CourseCompletions.Add(courseCompletion);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.IsCourseCompletedAsync(user.UserId, course.CourseId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task UpdateTimeSpentAsync_UpdatesTimeSpentForUser()
        {
            // Arrange
            var context = GetInMemoryDbContext(); 
            var user = new User { UserId = 1, Username = "Test User", CoinBalance = 100, LastLoginTime = DateTime.Now, NumberOfCompletedSections = 0 };
            context.Users.Add(user);
            await context.SaveChangesAsync(); 

            var repository = new Repository(context); 

            // Act
            await repository.UpdateTimeSpentAsync(user.UserId, 1, 500);  

            // Assert
            var updatedUser = await context.Users.FindAsync(user.UserId);
            Assert.AreEqual(500, updatedUser?.NumberOfCompletedSections);
        }

        [TestMethod]
        public async Task ClaimCompletionRewardAsync_ClaimsRewardSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext(); 
            var user = new User { UserId = 1, Username = "Test User", CoinBalance = 100, LastLoginTime = DateTime.Now };
            var course = new Course
            {
                CourseId = 1,
                Title = "Test Course",
                Description = "Test Course Description",
                IsPremium = false,
                Cost = 100,
                ImageUrl = "http://example.com/course.jpg",
                TimeToComplete = 3600,
                Difficulty = "Beginner"
            };
            var courseCompletion = new CourseCompletion
            {
                UserId = user.UserId,
                CourseId = course.CourseId,
                CompletionRewardClaimed = false,
                TimedRewardClaimed = false,
                CompletedAt = DateTime.Now
            };
            context.Users.Add(user);
            context.Courses.Add(course);
            context.CourseCompletions.Add(courseCompletion);
            await context.SaveChangesAsync();

            var repository = new Repository(context); 

            // Act
            await repository.ClaimCompletionRewardAsync(user.UserId, course.CourseId);

            // Assert
            var updatedCompletion = await context.CourseCompletions
                .FirstOrDefaultAsync(cc => cc.UserId == user.UserId && cc.CourseId == course.CourseId);
            Assert.IsTrue(updatedCompletion?.CompletionRewardClaimed); 
        }

        [TestMethod]
        public async Task ClaimTimeRewardAsync_ClaimsTimeRewardSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User { UserId = 1, Username = "Test User", CoinBalance = 100, LastLoginTime = DateTime.Now };
            var course = new Course
            {
                CourseId = 1,
                Title = "Test Course",
                Description = "Test Course Description",
                IsPremium = false,
                Cost = 100,
                ImageUrl = "http://example.com/course.jpg",
                TimeToComplete = 3600,
                Difficulty = "Beginner"
            };
            var courseCompletion = new CourseCompletion
            {
                UserId = user.UserId,
                CourseId = course.CourseId,
                CompletionRewardClaimed = false,
                TimedRewardClaimed = false,
                CompletedAt = DateTime.Now
            };
            context.Users.Add(user);
            context.Courses.Add(course);
            context.CourseCompletions.Add(courseCompletion);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.ClaimTimeRewardAsync(user.UserId, course.CourseId);

            // Assert
            var updatedCompletion = await context.CourseCompletions
                .FirstOrDefaultAsync(cc => cc.UserId == user.UserId && cc.CourseId == course.CourseId);
            Assert.IsTrue(updatedCompletion?.TimedRewardClaimed);
        }
        #endregion

        #region Exam
        [TestMethod]
        public async Task GetExamsFromDbAsync_ReturnsAllExams()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Exams.Add(new Exam { Id = 1, SectionId = 1 });
            context.Exams.Add(new Exam { Id = 2, SectionId = 2 });
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var exams = await repository.GetExamsFromDbAsync();

            // Assert
            Assert.AreEqual(2, exams.Count);
            Assert.IsTrue(exams.Any(e => e.Id == 1));
            Assert.IsTrue(exams.Any(e => e.Id == 2));
        }

        [TestMethod]
        public async Task GetExamByIdAsync_ReturnsExam_WhenExamExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var exam = new Exam { Id = 1, SectionId = 1 };
            context.Exams.Add(exam);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            var result = await repository.GetExamByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task AddExamAsync_AddsExamSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var exam = new Exam { SectionId = 1 };
            var repository = new Repository(context);

            // Act
            await repository.AddExamAsync(exam);
            var result = await context.Exams.FindAsync(exam.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(exam.SectionId, result?.SectionId);
        }

        [TestMethod]
        public async Task UpdateExamAsync_UpdatesExamSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var exam = new Exam { SectionId = 1 };
            context.Exams.Add(exam);
            await context.SaveChangesAsync();
            var repository = new Repository(context);

            // Act
            exam.SectionId = 2;
            await repository.UpdateExamAsync(exam);
            var updatedExam = await context.Exams.FindAsync(exam.Id);

            // Assert
            Assert.IsNotNull(updatedExam);
            Assert.AreEqual(2, updatedExam?.SectionId);
        }

        [TestMethod]
        public async Task DeleteExamAsync_DeletesExamSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var exam = new Exam { SectionId = 1 };
            context.Exams.Add(exam);
            await context.SaveChangesAsync();

            var repository = new Repository(context);

            // Act
            await repository.DeleteExamAsync(exam.Id);
            var deletedExam = await context.Exams.FindAsync(exam.Id);

            // Assert
            Assert.IsNull(deletedExam);
        }
        #endregion

    }
}
