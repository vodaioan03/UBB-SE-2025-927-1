using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DotNetEnv;
using Duo.Api.Persistence;
using Duo.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Duo.Api
{
    /// <summary>
    /// The entry point of the application.
    /// Configures services, middleware, and the HTTP request pipeline.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Program
    {
        #region Methods

        /// <summary>
        /// The main method, which serves as the entry point of the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            // Load environment variables from the .env file.
            Env.Load(".env");

            // Create a WebApplication builder.
            var builder = WebApplication.CreateBuilder(args);

            // Configure services for the application.
            ConfigureServices(builder);

            // Build the application.
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            ConfigureMiddleware(app);

            // Apply database migrations and seed data.
            ApplyMigrationsAndSeedData(app);

            // Run the application.
            app.Run();
        }

        /// <summary>
        /// Configures services for the application.
        /// </summary>
        /// <param name="builder">The WebApplication builder.</param>
        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            // Add controllers with JSON options.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                });

            // Register IRepository and Repository for dependency injection.
            builder.Services.AddScoped<IRepository, Repository>();

            // Configure Swagger/OpenAPI.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure the database context with a connection string.
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__WebApiDatabase");
            Console.WriteLine("Connection string: " + connectionString);

            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(connectionString));
        }

        /// <summary>
        /// Configures middleware for the application.
        /// </summary>
        /// <param name="app">The WebApplication instance.</param>
        private static void ConfigureMiddleware(WebApplication app)
        {
            // Enable Swagger in development environments.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Enable HTTPS redirection and authorization.
            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Map controllers to endpoints.
            app.MapControllers();
        }

        /// <summary>
        /// Applies database migrations and seeds initial data.
        /// </summary>
        /// <param name="app">The WebApplication instance.</param>
        private static void ApplyMigrationsAndSeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DataContext>();

            // Apply database migrations.
            db.Database.Migrate();

            // Seed initial data (e.g., add a default user if necessary).
            /* Uncomment and modify the following block if seeding is required.
            if (!db.Users.Any(u => u.UserId == 0))
            {
                db.Users.Add(new User
                {
                    UserId = 0,
                    CoinBalance = 1000, // Example balance
                    LastLoginTime = DateTime.Now
                });
                db.SaveChanges();
            }
            */
        }

        #endregion
    }
}
