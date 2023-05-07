using Autofac.Extensions.DependencyInjection;
using DataAccess.Utilities;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace WhoGivesACluck
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                var dbContext = new CluckContextFactory().CreateDbContext();
                dbContext.Database.Migrate();
                
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false)
                    .AddEnvironmentVariables()
                    .Build();


                Host.CreateDefaultBuilder(args)
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                    .ConfigureWebHostDefaults(builder =>
                    {
                        builder.UseStartup<Startup>();
                    })
                    .Build()
                    .Run();
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Startup failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
