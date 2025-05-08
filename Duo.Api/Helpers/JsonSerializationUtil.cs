using Duo.Api.Models.Exercises;
using Duo.Api.Models.Quizzes;
using Duo.Api.Repositories;
using System.Text.Json;

namespace Duo.Api.Helpers
{
    public class JsonSerializationUtil
    {
        public static string SerializeExamWithTypedExercises(Exam exam)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
            };

            var exerciseJsonList = new List<JsonDocument>();

            foreach (var exercise in exam.Exercises)
            {
                string exerciseJson = exercise.Type switch
                {
                    "Association" => JsonSerializer.Serialize((AssociationExercise)exercise, options),
                    "Flashcard" => JsonSerializer.Serialize((FlashcardExercise)exercise, options),
                    "MultipleChoice" => JsonSerializer.Serialize((MultipleChoiceExercise)exercise, options),
                    "FillInTheBlank" => JsonSerializer.Serialize((FillInTheBlankExercise)exercise, options),
                    _ => throw new InvalidOperationException($"Unknown exercise type: {exercise.Type}")
                };

                exerciseJsonList.Add(JsonDocument.Parse(exerciseJson));
            }

            var examDto = new
            {
                exam.Id,
                exam.SectionId,
                Exercises = exerciseJsonList.Select(doc => doc.RootElement).ToList(),
            };

            return JsonSerializer.Serialize(examDto, options);
        }

        public static async Task<Exam> DeserializeExamWithTypedExercises(string json, IRepository repo)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            using JsonDocument doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            Console.WriteLine($"Root Element Type: {root.ValueKind}"); // Check root type


            int id = root.GetProperty("Id").GetInt32();
            int? sectionId = root.TryGetProperty("SectionId", out var sectionProp) && sectionProp.ValueKind != JsonValueKind.Null
                ? sectionProp.GetInt32()
                : null;

            var exam = new Exam
            {
                Id = id,
                SectionId = sectionId,
                Exercises = new List<Exercise>()
            };

            var exerciseIds = root.GetProperty("Exercises").EnumerateArray().Select(e => e.GetInt32());
            foreach (var exerciseId in exerciseIds)
            {
                var exercise = await repo.GetExerciseByIdAsync(exerciseId);
                if (exercise == null)
                {
                    throw new InvalidOperationException($"Exercise with ID {exerciseId} not found.");
                }

                exam.Exercises.Add(exercise);
            }

            return exam;
        }
    }
}
