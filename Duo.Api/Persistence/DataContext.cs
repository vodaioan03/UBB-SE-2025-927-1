using CourseApp.Models;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Microsoft.EntityFrameworkCore;

namespace Duo.Api.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Module> Modules { get; set; }

        public DbSet<BaseQuiz> Quizzes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure TPH inheritance for quizzes
            modelBuilder.Entity<BaseQuiz>()
                .HasDiscriminator<string>("QuizType")
                .HasValue<Quiz>("Quiz")
                .HasValue<Exam>("Exam");

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