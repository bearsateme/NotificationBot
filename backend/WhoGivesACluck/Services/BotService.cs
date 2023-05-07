using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WhoGivesACluck.Commands;
using WhoGivesACluck.Models.Enums;

namespace WhoGivesACluck.Services
{
    public class BotService : BackgroundService
    {  
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly DiscordClient _discord;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BotService> Logger;
        
        public BotService(IConfiguration configuration,  IHostApplicationLifetime applicationLifetime, ILogger<BotService> logger)
        {
            _applicationLifetime = applicationLifetime;
            _configuration = configuration;
            _discord = new DiscordClient(new DiscordConfiguration
            {
                Token = configuration["Bot_Key"],
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents | DiscordIntents.Guilds
            });
          
            Logger = logger;
        }
        
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            List<ulong> generalChannels= new List<ulong>();
            await _discord.ConnectAsync();

            _discord.GuildDownloadCompleted += (sender, args) =>
            {
                var guilds = _discord.Guilds.Values;
                foreach (var guild in guilds)
                {
                    var channels = guild.Channels;

                    foreach (var channel in channels)
                    {
                        if (channel.Value.Name == "general")
                        {
                            generalChannels.Add(channel.Key);
                        }
                    }
                }

                return Task.CompletedTask;
            };

            var commands = _discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { "!" },
            });

            commands.RegisterCommands<FetchCommandModule>();

            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                
                var timeUtc = DateTime.UtcNow;
                var easternZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
                var easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
                var result = await Utility.GetResult($"https://statsapi.web.nhl.com/api/v1/schedule?teamId=12&date={easternTime:yyyy-MM-dd}");

                if (result.Status is GameStatus.Away or GameStatus.None)
                {
                    await Task.Delay(86400000, cancellationToken);
                    continue;
                }

                if (result.Status != GameStatus.Finished)
                {
                    await Task.Delay(300000, cancellationToken);
                    continue;
                }

                if (result.HomeScore > result.AwayScore)
                {
                    foreach (var generalChannel in generalChannels)
                    {
                        var channel = await _discord.GetChannelAsync(generalChannel);
                        await channel.SendMessageAsync("get your shit");    
                    }

                    await Task.Delay(86400000, cancellationToken);
                }
                else
                {
                    foreach (var generalChannel in generalChannels)
                    {
                        var channel = await _discord.GetChannelAsync(generalChannel);
                        await channel.SendMessageAsync("we lost");    
                    }
                    await Task.Delay(86400000, cancellationToken);
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _discord.DisconnectAsync();
        }
    }
}
