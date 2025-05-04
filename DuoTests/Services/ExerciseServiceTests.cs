using Microsoft.VisualStudio.TestTools.UnitTesting;
using Duo.Services;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Duo.Models;
using System.Diagnostics.CodeAnalysis;

namespace Duo.Tests.Services
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ExerciseServiceTests
    {
        private Mock<IExerciseService> _mockProxy;
        private ExerciseService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockProxy = new Mock<IExerciseService>();
            _service = new ExerciseService(_mockProxy.Object);
        }

        // Define concrete test class for Exercise
        public class TestExercise : Exercise
        {
            public TestExercise(int id, string question, Difficulty difficulty)
                : base(id, question, difficulty)
            {
            }
        }

        [TestMethod]
        public async Task GetAllExercises_ProxyReturnsList_ReturnsSameList()
        {
            var expected = new List<Exercise>
            {
                new TestExercise(1, "Q1", Difficulty.Easy),
                new TestExercise(2, "Q2", Difficulty.Normal)
            };
            _mockProxy.Setup(p => p.GetAllExercises()).ReturnsAsync(expected);

            var result = await _service.GetAllExercises();

            Assert.AreEqual(expected.Count, result.Count);
            Assert.AreEqual(expected[0].Id, result[0].Id);
        }

        [TestMethod]
        public async Task GetAllExercises_ProxyThrowsException_ReturnsEmptyList()
        {
            _mockProxy.Setup(p => p.GetAllExercises()).ThrowsAsync(new Exception("error"));

            var result = await _service.GetAllExercises();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetExerciseById_ValidId_ReturnsExercise()
        {
            var exercise = new TestExercise(42, "Sample", Difficulty.Normal);
            _mockProxy.Setup(p => p.GetExerciseById(42)).ReturnsAsync(exercise);

            var result = await _service.GetExerciseById(42);

            Assert.IsNotNull(result);
            Assert.AreEqual(42, result.Id);
        }

        [TestMethod]
        public async Task GetExerciseById_ProxyThrowsException_ReturnsNull()
        {
            _mockProxy.Setup(p => p.GetExerciseById(1)).ThrowsAsync(new Exception());

            var result = await _service.GetExerciseById(1);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllExercisesFromQuiz_ValidQuizId_ReturnsList()
        {
            var expected = new List<Exercise> { new TestExercise(1, "Quiz Q", Difficulty.Easy) };
            _mockProxy.Setup(p => p.GetAllExercisesFromQuiz(5)).ReturnsAsync(expected);

            var result = await _service.GetAllExercisesFromQuiz(5);

            Assert.AreEqual(expected.Count, result.Count);
        }

        [TestMethod]
        public async Task GetAllExercisesFromQuiz_ProxyThrowsException_ReturnsEmptyList()
        {
            _mockProxy.Setup(p => p.GetAllExercisesFromQuiz(5)).ThrowsAsync(new Exception());

            var result = await _service.GetAllExercisesFromQuiz(5);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetAllExercisesFromExam_ValidExamId_ReturnsList()
        {
            var expected = new List<Exercise> { new TestExercise(2, "Exam Q", Difficulty.Hard) };
            _mockProxy.Setup(p => p.GetAllExercisesFromExam(3)).ReturnsAsync(expected);

            var result = await _service.GetAllExercisesFromExam(3);

            Assert.AreEqual(expected.Count, result.Count);
        }

        [TestMethod]
        public async Task GetAllExercisesFromExam_ProxyThrowsException_ReturnsEmptyList()
        {
            _mockProxy.Setup(p => p.GetAllExercisesFromExam(3)).ThrowsAsync(new Exception());

            var result = await _service.GetAllExercisesFromExam(3);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task DeleteExercise_ValidId_CallsProxyOnce()
        {
            await _service.DeleteExercise(7);

            _mockProxy.Verify(p => p.DeleteExercise(7), Times.Once);
        }

        [TestMethod]
        public async Task DeleteExercise_ProxyThrowsException_DoesNotThrow()
        {
            _mockProxy.Setup(p => p.DeleteExercise(It.IsAny<int>())).ThrowsAsync(new Exception());

            await _service.DeleteExercise(7);

            _mockProxy.Verify(p => p.DeleteExercise(7), Times.Once);
        }

        [TestMethod]
        public async Task CreateExercise_ValidExercise_CallsProxy()
        {
            var exercise = new AssociationExercise(
                11, "Fail Q", Difficulty.Hard, new List<String> { "A", "B", "C" }, new List<String> { "D", "E", "F" });
            await _service.CreateExercise(exercise);

            _mockProxy.Verify(p => p.CreateExercise(exercise), Times.Once);
        }

        [TestMethod]
        public async Task CreateExercise_ProxyThrowsException_DoesNotThrow()
        {
            var exercise = new AssociationExercise(
                11, "Fail Q", Difficulty.Hard, new List<String> { "A", "B", "C" }, new List<String> { "D", "E", "F" });

            _mockProxy.Setup(p => p.CreateExercise(exercise)).ThrowsAsync(new Exception());

            await _service.CreateExercise(exercise);

            _mockProxy.Verify(p => p.CreateExercise(exercise), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullProxy_ThrowsArgumentNullException()
        {
            // Act
            var service = new ExerciseService(null);
        }
    }
}
