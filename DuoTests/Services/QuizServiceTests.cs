//using Xunit;
//using Moq;
//using Duo.Services;
//using Duo.Models.Quizzes;
//using Duo.Models.Exercises;
//using Duo.Models.Quizzes.API;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace Duo.Tests.Services
//{
//    public class QuizServiceTests
//    {
//        private readonly Mock<QuizServiceProxy> mockProxy;
//        private readonly QuizService quizService;

//        public QuizServiceTests()
//        {
//            mockProxy = new Mock<QuizServiceProxy>();
//            quizService = new QuizService(mockProxy.Object);
//        }

//        [Fact]
//        public async Task Get_ReturnsAllQuizzes()
//        {
//            var quizzes = new List<Quiz> { new Quiz { Id = 1 }, new Quiz { Id = 2 } };
//            mockProxy.Setup(p => p.GetAsync()).ReturnsAsync(quizzes);

//            var result = await quizService.Get();

//            Assert.Equal(2, result.Count);
//        }

//        [Fact]
//        public async Task GetQuizById_ReturnsQuiz()
//        {
//            var quiz = new Quiz { Id = 5 };
//            mockProxy.Setup(p => p.GetQuizByIdAsync(5)).ReturnsAsync(quiz);

//            var result = await quizService.GetQuizById(5);

//            Assert.Equal(5, result.Id);
//        }

//        [Fact]
//        public async Task CountQuizzesFromSection_ReturnsCorrectCount()
//        {
//            mockProxy.Setup(p => p.CountQuizzesFromSectionAsync(3)).ReturnsAsync(4);

//            var count = await quizService.CountQuizzesFromSection(3);

//            Assert.Equal(4, count);
//        }

//        [Fact]
//        public async Task CreateQuiz_ReturnsQuizId()
//        {
//            var quiz = new Quiz { Id = 10 };
//            mockProxy.Setup(p => p.CreateQuizAsync(quiz)).Returns(Task.CompletedTask);

//            var result = await quizService.CreateQuiz(quiz);

//            Assert.Equal(10, result);
//        }

//        [Fact]
//        public async Task AddExercisesToQuiz_CallsProxyWithExerciseIds()
//        {
//            var exercises = new List<Exercise>
//            {
//                new Exercise { Id = 1 },
//                new Exercise { Id = 2 }
//            };

//            mockProxy.Setup(p => p.AddExercisesToQuizAsync(1, It.Is<List<int>>(ids => ids.Contains(1) && ids.Contains(2))))
//                     .Returns(Task.CompletedTask);

//            await quizService.AddExercisesToQuiz(1, exercises);

//            mockProxy.Verify(p => p.AddExercisesToQuizAsync(1, It.IsAny<List<int>>()), Times.Once);
//        }

//        [Fact]
//        public async Task SubmitQuizAsync_CallsProxyOnce()
//        {
//            var submission = new QuizSubmission { QuizId = 1 };

//            await quizService.SubmitQuizAsync(submission);

//            mockProxy.Verify(p => p.SubmitQuizAsync(submission), Times.Once);
//        }

//        [Fact]
//        public async Task GetResultAsync_ReturnsQuizResult()
//        {
//            var result = new QuizResult { QuizId = 42, TotalQuestions = 5, CorrectAnswers = 4 };
//            mockProxy.Setup(p => p.GetResultAsync(42)).ReturnsAsync(result);

//            var actual = await quizService.GetResultAsync(42);

//            Assert.Equal(42, actual.QuizId);
//            Assert.Equal(5, actual.TotalQuestions);
//            Assert.Equal(4, actual.CorrectAnswers);
//        }
//    }
//}