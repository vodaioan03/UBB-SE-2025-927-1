using DotNetEnv;
using System.Text.Json.Serialization;
using Duo.Api.Persistence;
using Microsoft.EntityFrameworkCore;
using Duo.Api.Repositories;
using Duo.Api.Models;
using Duo.Api.Repositories;

namespace Duo.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Env.Load(".env");

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                });

            // Register the IRepository and Repository for dependency injection
            builder.Services.AddScoped<IRepository, Repository>();  // Register IRepository with Repository

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__WebApiDatabase");
            Console.WriteLine("Connection string: " + connectionString);

            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IRepository, Repository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();

                // Add a user with ID 0 if it doesn't exist
/*                if (!db.Users.Any(u => u.UserId == 0))
                {
                    db.Users.Add(new User
                    {
                        UserId = 0,
                        CoinBalance = 1000, // Example balance
                        LastLoginTime = DateTime.Now
                    });
                    db.SaveChanges();
                }*/

                db.Database.Migrate(); // Ensure migrations are applied
            }

            app.Run();
        }
    }
}