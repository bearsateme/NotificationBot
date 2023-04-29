﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationBot.Services;

namespace NotificationBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration config = new ConfigurationBuilder()
                        .AddEnvironmentVariables()
                        .AddCommandLine(args)
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false)
                        .AddJsonFile($"appsettings.Development.json", optional: true)
                        .Build();
                    
                    services.AddSingleton(config);
                    services.AddHostedService<BotService>();
                })
                .Build();
            
            await host.RunAsync();
        }
    }
}