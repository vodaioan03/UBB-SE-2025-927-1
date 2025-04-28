using Duo.Api.Models;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Quizzes;
using Duo.Api.Models.Sections;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Duo.Api.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Module> Modules { get; set; }

        public DbSet<CourseCompletion> CourseCompletions { get; set; }
        public DbSet<Course> Courses { get; set; }

        public DbSet<Exam> Exams { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        
        public DbSet<Section> Sections {get;set;}

        public DbSet<Exercise> Exercises { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ================================
            // Section and Quiz Relationships
            // ================================

            // Section -> Quizzes (one-to-many)
            modelBuilder.Entity<Section>()
                .HasMany(s => s.Quizzes)
                .WithOne(q => q.Section)
                .HasForeignKey(q => q.SectionId)
                .OnDelete(DeleteBehavior.SetNull);

            // Section -> Exam (one-to-one)
            modelBuilder.Entity<Section>()
                .HasOne(s => s.Exam)
                .WithOne(e => e.Section)
                .HasForeignKey<Exam>(e => e.SectionId)
                .OnDelete(DeleteBehavior.SetNull);

            // Quiz -> Exercises (many-to-many)
            modelBuilder.Entity<Quiz>()
                .HasMany(q => q.Exercises)
                .WithMany(e => e.Quizzes)
                .UsingEntity(j => j.ToTable("QuizExercises"));

            // Exam -> Exercises (many-to-many)
            modelBuilder.Entity<Exam>()
                .HasMany(e => e.Exercises)
                .WithMany(e => e.Exams)
                .UsingEntity(j => j.ToTable("ExamExercises"));

            // ================================
            // Exercise Hierarchy and Configuration
            // ================================

            // Table-Per-Hierarchy (TPH) for Exercises
            modelBuilder.Entity<Exercise>()
                .HasDiscriminator<string>("ExerciseType")
                .HasValue<AssociationExercise>("Association")
                .HasValue<FillInTheBlankExercise>("Fill in the blank")
                .HasValue<FlashcardExercise>("Flashcard")
                .HasValue<MultipleChoiceExercise>("Multiple Choice");

            // Index for Exercise Question (search optimization)
            modelBuilder.Entity<Exercise>()
                .HasIndex(e => e.Question)
                .HasDatabaseName("IX_Exercise_Question");

            // ================================
            // Exercise Specific Properties
            // ================================

            // AssociationExercise -> FirstAnswersList and SecondAnswersList (stored as JSON)
            modelBuilder.Entity<AssociationExercise>()
                .Property(a => a.FirstAnswersList)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null));

            modelBuilder.Entity<AssociationExercise>()
                .Property(a => a.SecondAnswersList)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null));

            // FillInTheBlankExercise -> PossibleCorrectAnswers (stored as JSON)
            modelBuilder.Entity<FillInTheBlankExercise>()
                .Property(a => a.PossibleCorrectAnswers)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null));

            // MultipleChoiceExercise -> MultipleChoiceAnswerModel (real one-to-many)
            modelBuilder.Entity<MultipleChoiceExercise>()
                .HasMany(m => m.Choices)
                .WithOne(c => c.Exercise)
                .HasForeignKey(c => c.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Course -> CourseCompleion (one-to-many)
            modelBuilder.Entity<CourseCompletion>()
                .HasKey(cc => new { cc.UserId, cc.CourseId });
        }
    }
}