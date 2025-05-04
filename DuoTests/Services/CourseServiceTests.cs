using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Duo.Models;
using Duo.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Services.Interfaces;

namespace Duo.Tests.Services
{
    [TestClass]
    public class CourseServiceTests
    {
        private Mock<ICourseServiceProxy> mockProxy ;
        private CourseService courseService;

        [TestInitialize]
        public void Setup()
        {
            mockProxy = new Mock<ICourseServiceProxy>();
            courseService = new CourseService(mockProxy.Object);
        }

        private Course CreateSampleCourse(int id) => new Course
        {
            CourseId = id,
            Title = $"Title {id}",
            Description = $"Description {id}",
            ImageUrl = $"https://example.com/image{id}.jpg",
            Difficulty = "Beginner"
        };

        [TestMethod]
        public async Task GetCoursesAsync_ReturnsCourses()
        {
            var expected = new List<Course> { CreateSampleCourse(1), CreateSampleCourse(2) };
            mockProxy.Setup(p => p.GetAllCourses()).ReturnsAsync(expected);

            var result = await courseService.GetCoursesAsync();

            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetTagsAsync_ReturnsTags()
        {
            var expected = new List<Tag> { new Tag { TagId = 1 } };
            mockProxy.Setup(p => p.GetAllTags()).ReturnsAsync(expected);

            var result = await courseService.GetTagsAsync();

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task GetCourseTagsAsync_ReturnsTags()
        {
            var expected = new List<Tag> { new Tag { TagId = 3 } };
            mockProxy.Setup(p => p.GetTagsForCourse(2)).ReturnsAsync(expected);

            var result = await courseService.GetCourseTagsAsync(2);

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task OpenModuleAsync_OpensIfNotAlreadyOpen()
        {
            mockProxy.Setup(p => p.IsModuleOpen(1, 2)).ReturnsAsync(false);

            await courseService.OpenModuleAsync(1, 2);

            mockProxy.Verify(p => p.OpenModule(1, 2), Times.Once);
        }

        [TestMethod]
        public async Task GetModulesAsync_ReturnsModules()
        {
            var expected = new List<Module> { new Module { ModuleId = 1, Title = "title1", Description = "description1", ImageUrl = "url1" } };
            mockProxy.Setup(p => p.GetModulesByCourseId(2)).ReturnsAsync(expected);

            var result = await courseService.GetModulesAsync(2);

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task GetNormalModulesAsync_ReturnsNonBonusModules()
        {
            var modules = new List<Module>
            {
                new Module { ModuleId = 1, IsBonus = false, Title = "title1", Description = "description1", ImageUrl = "url1" },
                new Module { ModuleId = 2, IsBonus = true, Title = "title2", Description = "description2", ImageUrl = "url2" }
            };
            mockProxy.Setup(p => p.GetModulesByCourseId(1)).ReturnsAsync(modules);

            var result = await courseService.GetNormalModulesAsync(1);

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task EnrollInCourseAsync_EnrollsIfNotAlreadyEnrolled()
        {
            mockProxy.Setup(p => p.IsUserEnrolled(1, 2)).ReturnsAsync(false);

            var result = await courseService.EnrollInCourseAsync(1, 2);

            mockProxy.Verify(p => p.EnrollUser(1, 2), Times.Once);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task CompleteModuleAsync_MarksCourseIfCompleted()
        {
            mockProxy.Setup(p => p.IsCourseCompleted(1, 2)).ReturnsAsync(true);

            await courseService.CompleteModuleAsync(1, 3, 2);

            mockProxy.Verify(p => p.MarkCourseAsCompleted(1, 2), Times.Once);
        }

        [TestMethod]
        public async Task IsUserEnrolledAsync_ReturnsCorrectStatus()
        {
            mockProxy.Setup(p => p.IsUserEnrolled(1, 2)).ReturnsAsync(true);

            var result = await courseService.IsUserEnrolledAsync(1, 2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsModuleCompletedAsync_ReturnsCorrectStatus()
        {
            mockProxy.Setup(p => p.IsModuleCompleted(1, 2)).ReturnsAsync(true);

            var result = await courseService.IsModuleCompletedAsync(1, 2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ClickModuleImageAsync_ReturnsTrueIfClicked()
        {
            mockProxy.Setup(p => p.IsModuleImageClicked(1, 2)).ReturnsAsync(false);

            var result = await courseService.ClickModuleImageAsync(1, 2);

            mockProxy.Verify(p => p.ClickModuleImage(1, 2), Times.Once);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsModuleInProgressAsync_ReturnsTrueIfOpen()
        {
            mockProxy.Setup(p => p.IsModuleOpen(1, 2)).ReturnsAsync(true);

            var result = await courseService.IsModuleInProgressAsync(1, 2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsModuleAvailableAsync_ReturnsCorrectStatus()
        {
            mockProxy.Setup(p => p.IsModuleAvailable(1, 2)).ReturnsAsync(true);

            var result = await courseService.IsModuleAvailableAsync(1, 2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsCourseCompletedAsync_ReturnsCorrectStatus()
        {
            mockProxy.Setup(p => p.IsCourseCompleted(1, 2)).ReturnsAsync(true);

            var result = await courseService.IsCourseCompletedAsync(1, 2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task GetCompletedModulesCountAsync_ReturnsCount()
        {
            mockProxy.Setup(p => p.GetCompletedModulesCount(1, 2)).ReturnsAsync(3);

            var result = await courseService.GetCompletedModulesCountAsync(1, 2);

            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public async Task GetRequiredModulesCountAsync_ReturnsCount()
        {
            mockProxy.Setup(p => p.GetRequiredModulesCount(2)).ReturnsAsync(5);

            var result = await courseService.GetRequiredModulesCountAsync(2);

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public async Task UpdateTimeSpentAsync_CallsProxy()
        {
            await courseService.UpdateTimeSpentAsync(1, 2, 100);

            mockProxy.Verify(p => p.UpdateTimeSpent(1, 2, 100), Times.Once);
        }

        [TestMethod]
        public async Task GetTimeSpentAsync_ReturnsTime()
        {
            mockProxy.Setup(p => p.GetTimeSpent(1, 2)).ReturnsAsync(300);

            var result = await courseService.GetTimeSpentAsync(1, 2);

            Assert.AreEqual(300, result);
        }

        [TestMethod]
        public async Task ClaimCompletionRewardAsync_ReturnsTrueIfSuccessful()
        {
            mockProxy.Setup(p => p.ClaimCompletionReward(1, 2)).ReturnsAsync(true);

            var result = await courseService.ClaimCompletionRewardAsync(1, 2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ClaimTimedRewardAsync_ReturnsTrueIfSuccessful()
        {
            mockProxy.Setup(p => p.ClaimTimedReward(1, 2, 1000)).ReturnsAsync(true);

            var result = await courseService.ClaimTimedRewardAsync(1, 2, 1000);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task GetCourseTimeLimitAsync_ReturnsTimeLimit()
        {
            mockProxy.Setup(p => p.GetCourseTimeLimit(2)).ReturnsAsync(3600);

            var result = await courseService.GetCourseTimeLimitAsync(2);

            Assert.AreEqual(3600, result);
        }

        [TestMethod]
        public async Task BuyBonusModuleAsync_ReturnsTrueIfPurchased()
        {
            mockProxy.Setup(p => p.BuyBonusModule(1, 2, 3)).ReturnsAsync(true);

            var result = await courseService.BuyBonusModuleAsync(1, 2, 3);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task GetFilteredCoursesAsync_ReturnsFilteredCourses()
        {
            var course = CreateSampleCourse(1);
            var tag = new Tag { TagId = 10 };

            mockProxy.Setup(p => p.GetAllCourses()).ReturnsAsync(new List<Course> { course });
            mockProxy.Setup(p => p.IsUserEnrolled(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);
            mockProxy.Setup(p => p.GetTagsForCourse(course.CourseId)).ReturnsAsync(new List<Tag> { tag });

            var result = await courseService.GetFilteredCoursesAsync("test", false, true, false, true, new List<int> { 10 }, 1);

            Assert.AreEqual(1, result.Count);
        }
    }
}
