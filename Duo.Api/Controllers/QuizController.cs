
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Duo.Api.Models.Quizzes;
using Duo.Api.Persistence;
using Duo.Api.Repositories;
using Duo.Api.Models.Exercises;
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

namespace Duo.Api.Controllers
{
    /// <summary>
    /// Controller for managing quizzes in the system.
    /// Provides endpoints for CRUD operations and additional quiz-related functionalities.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="QuizController"/> class with the specified repository.
    /// </remarks>
    /// <param name="repository">The repository instance for data access.</param>
    [ApiController]
    [Route("quiz")]
    [ExcludeFromCodeCoverage]
    public class QuizController(IRepository repository) : BaseController(repository)
    {
        private readonly IRepository repository = repository;

        #region Public Methods

        /// <summary>
        /// Adds a new quiz to the database.
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> AddQuiz([FromForm] Quiz quiz)
        {
            await repository.AddQuizAsync(quiz);
            return Ok(quiz);
        }

        /// <summary>
        /// Retrieves a quiz by its ID.
        /// </summary>
        [HttpGet("get")]
        public async Task<IActionResult> GetQuiz([FromQuery] int id)
        {
            var quiz = await repository.GetQuizByIdAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            return Ok(quiz);
        }

        /// <summary>
        /// Lists all quizzes in the database.
        /// </summary>
        [HttpGet("list")]
        public async Task<IActionResult> ListQuizzes()
        {
            var quizzes = await repository.GetQuizzesFromDbAsync();
            return Ok(quizzes);
        }

        /// <summary>
        /// Updates an existing quiz.
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateQuiz([FromForm] Quiz updatedQuiz)
        {
            var quiz = await repository.GetQuizByIdAsync(updatedQuiz.Id);
            if (quiz == null)
            {
                return NotFound();
            }

            await repository.UpdateQuizAsync(updatedQuiz);
            return Ok(updatedQuiz);
        }

        /// <summary>
        /// Deletes a quiz by its ID.
        /// </summary>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteQuiz([FromQuery] int id)
        {
            var quiz = await repository.GetQuizByIdAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            await repository.DeleteQuizAsync(id);
            return Ok();
        }

        /// <summary>
        /// Retrieves all quizzes from a specific section.
        /// </summary>
        [HttpGet("get-all-section")]
        public async Task<IActionResult> GetAllQuizzesFromSection([FromQuery] int sectionId)
        {
            var quizzes = await repository.GetAllQuizzesFromSectionAsync(sectionId);
            return Ok(quizzes);
        }

        /// <summary>
        /// Retrieves the number of quizzes in a specific section.
        /// </summary>
        [HttpGet("count-from-section")]
        public async Task<IActionResult> CountQuizzesFromSection([FromQuery] int sectionId)
        {
            var count = await repository.CountQuizzesFromSectionAsync(sectionId);
            return Ok(count);
        }

        /// <summary>
        /// Retrieves the last order number from a specific section.
        /// </summary>
        [HttpGet("last-order")]
        public async Task<IActionResult> GetLastOrderNumberFromSection([FromQuery] int sectionId)
        {
            var lastOrder = await repository.GetLastOrderNumberFromSectionAsync(sectionId);
            return Ok(lastOrder);
        }

        /// <summary>
        /// Adds a list of new exercises to a quiz.
        /// </summary>
        [HttpPost("add-exercises")]
        public async Task<IActionResult> AddExercisesToQuiz([FromForm] int quizId, [FromForm] List<int> exercises)
        {
            await repository.AddExercisesToQuizAsync(quizId, exercises);
            return Ok();
        }

        /// <summary>
        /// Adds a single exercise to a quiz.
        /// </summary>
        [HttpPost("add-exercise")]
        public async Task<IActionResult> AddExerciseToQuiz([FromForm] int quizId, [FromForm] int exerciseId)
        {
            await repository.AddExerciseToQuizAsync(quizId, exerciseId);
            return Ok();
        }

        /// <summary>
        /// Removes an exercise from a quiz.
        /// </summary>
        [HttpDelete("remove-exercise")]
        public async Task<IActionResult> RemoveExerciseFromQuiz([FromQuery] int quizId, [FromQuery] int exerciseId)
        {
            await repository.RemoveExerciseFromQuizAsync(quizId, exerciseId);
            return Ok();
        }

        /// <summary>
        /// Submits a quiz with user's answers, checks correctness, and saves the submission.
        /// </summary>
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitQuiz([FromBody] QuizSubmission submission)
        {
            var quiz = await repository.GetQuizByIdAsync(submission.QuizId);
            if (quiz == null)
                return NotFound();

            var submissionEntity = new QuizSubmissionEntity
            {
                QuizId = submission.QuizId,
                StartTime = DateTime.Now.AddMinutes(-5),
                EndTime = DateTime.Now,
            };

            foreach (var answer in submission.Answers)
            {
                var question = quiz.Exercises.FirstOrDefault(q => q.ExerciseId == answer.QuestionId);
                if (question != null)
                {
                    submissionEntity.Answers.Add(new AnswerSubmissionEntity
                    {
                        QuestionId = answer.QuestionId,
                        IsCorrect = CheckIfCorrect(question, answer)
                    });
                }
            }

            await repository.SaveQuizSubmissionAsync(submissionEntity);
            return Ok();
        }

        /// <summary>
        /// Checks if a given answer is correct depending on the type of exercise.
        /// </summary>
        /// <param name="question">The exercise question to check against.</param>
        /// <param name="userAnswer">The user's submitted answer.</param>
        /// <returns>True if the answer is correct, otherwise false.</returns>
        private bool CheckIfCorrect(Exercise question, AnswerSubmission userAnswer)
        {
            if (question is MultipleChoiceExercise mcq)
            {
                if (userAnswer.SelectedOptionIndex.HasValue &&
                    mcq.Choices != null &&
                    userAnswer.SelectedOptionIndex.Value >= 0 &&
                    userAnswer.SelectedOptionIndex.Value < mcq.Choices.Count)
                {
                    return mcq.Choices[userAnswer.SelectedOptionIndex.Value].IsCorrect;
                }
            }
            else if (question is FillInTheBlankExercise fill)
            {
                return fill.PossibleCorrectAnswers
                    .Any(correct => string.Equals(correct, userAnswer.WrittenAnswer?.Trim(), StringComparison.OrdinalIgnoreCase));
            }
            else if (question is FlashcardExercise flash)
            {
                return flash.ValidateAnswer(userAnswer.WrittenAnswer ?? string.Empty);
            }
            else if (question is AssociationExercise association)
            {
                if (userAnswer.SelectedOptionIndex.HasValue && userAnswer.AssociatedPairId.HasValue)
                {
                    var selectedFirstIndex = userAnswer.SelectedOptionIndex.Value;
                    var selectedSecondIndex = userAnswer.AssociatedPairId.Value;

                    if (selectedFirstIndex >= 0 && selectedFirstIndex < association.FirstAnswersList.Count &&
                        selectedSecondIndex >= 0 && selectedSecondIndex < association.SecondAnswersList.Count)
                    {
                        return selectedFirstIndex == selectedSecondIndex;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Retrieves the quiz result (total questions, correct answers, and time taken) after submission.
        /// </summary>
        [HttpGet("get-result")]
        public async Task<IActionResult> GetQuizResult([FromQuery] int quizId)
        {
            var submission = await repository.GetSubmissionByQuizIdAsync(quizId);
            if (submission == null)
                return NotFound();

            var result = new QuizResult
            {
                QuizId = quizId,
                TotalQuestions = submission.Answers.Count,
                CorrectAnswers = submission.Answers.Count(a => a.IsCorrect),
                TimeTaken = submission.EndTime - submission.StartTime
            };

            return Ok(result);
        }

        #endregion
    }
}
