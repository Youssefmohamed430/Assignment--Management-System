using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Assignment__Management_System.Models.Data
{
    public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            Env.TraversePath().Load();

            var connectionString = Environment.GetEnvironmentVariable("constr");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException(
                    "constr is missing. Add it to the backend .env file or set it as an environment variable.");

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure())
                .Options;

            return new AppDbContext(options);
        }
    }
}
