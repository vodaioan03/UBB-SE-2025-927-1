using CourseApp.Models;
using Duo.Api.Models;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Quizzes;
using Microsoft.EntityFrameworkCore;

namespace Duo.Api.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Module> Modules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure TPH inheritance for quizzes
            modelBuilder.Entity<BaseQuiz>()
                .HasDiscriminator<string>("QuizType")
                .HasValue<Quiz>("Quiz")
                .HasValue<Exam>("Exam");

            // Configure TPH inheritance for exercises
            modelBuilder.Entity<Exercise>()
                .HasDiscriminator<string>("ExerciseType")
                .HasValue<AssociationExercise>("Association")
                .HasValue<FillInTheBlankExercise>("Fill in the blank")
                .HasValue<FlashcardExercise>("Flashcard")
                .HasValue<MultipleChoiceExercise>("Multiple Choice");

            // Configure indexes for search optimization
            modelBuilder.Entity<Exercise>()
                .HasIndex(e => e.Question)
                .HasDatabaseName("IX_Exercise_Question");

            // Configure the many-to-many relationship between quizzes and exercises
            modelBuilder.Entity<BaseQuiz>()
                .HasMany(bq => bq.ExerciseList)
                .WithMany(e => e.Quizzes)
                .UsingEntity<Dictionary<string, object>>(
                    "QuizExercises",
                    j => j.HasOne<Exercise>().WithMany().HasForeignKey("ExerciseId"),
                    j => j.HasOne<BaseQuiz>().WithMany().HasForeignKey("QuizId")
                );
         }

    }
}