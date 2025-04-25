using CourseApp.Models;
using Duo.Api.Models;
using Duo.Models;
using Microsoft.EntityFrameworkCore;

namespace Duo.Api.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Module> Modules { get; set; }


        // Add Course-related models
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseCompletion> CourseCompletions { get; set; }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key for CourseCompletion: UserId + CourseId
            modelBuilder.Entity<CourseCompletion>()
                .HasKey(cc => new { cc.UserId, cc.CourseId });

            // Configure relationships
            modelBuilder.Entity<CourseCompletion>()
                .HasOne(cc => cc.Course)
                .WithMany()
                .HasForeignKey(cc => cc.CourseId);

            modelBuilder.Entity<CourseCompletion>()
                .HasOne(cc => cc.User)
                .WithMany()
                .HasForeignKey(cc => cc.UserId);
        }*/

    }
}