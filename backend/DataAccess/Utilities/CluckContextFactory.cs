using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Utilities
{
    public class CluckContextFactory : IDesignTimeDbContextFactory<CluckContext>
    {
        private static volatile string _connectionString;
        private static object _mutex = new object();
        
        public CluckContext CreateDbContext()
        {
            return CreateDbContext(null);
        }

        public CluckContext CreateDbContext(params string[] args)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                lock (_mutex)
                {
                    if (string.IsNullOrEmpty(_connectionString))
                    {
                        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                        var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false)
                            .AddEnvironmentVariables();

                        var config = builder.Build();

                        _connectionString = config.GetConnectionString("Default");
                    }
                }
            }

            var optionsBuilder = new DbContextOptionsBuilder<CluckContext>();
            optionsBuilder.UseSqlite(_connectionString);
            return new CluckContext(optionsBuilder.Options, this);
        }
    }
}