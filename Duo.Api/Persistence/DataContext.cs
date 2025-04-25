using CourseApp.Models;
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
    }
}