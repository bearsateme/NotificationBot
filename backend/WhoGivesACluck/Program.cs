using WhoGivesACluck.Services;

var builder = WebApplication.CreateBuilder(args);

IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile("appsettings.Development.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddSingleton(config);
builder.Services.AddControllers();
builder.Services.AddHostedService<BotService>();
builder.Services.AddMvc();

var app = builder.Build();

app.MapControllers();

app.Run();