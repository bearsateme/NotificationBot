using BusinessLogic.Interfaces;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Models;
using Models.Enums;
using WhoGivesACluck.Commands;

namespace WhoGivesACluck.Services
{
    public class BotService : BackgroundService
    {  
        private readonly DiscordClient _discord;
        private readonly IServiceProvider _serviceProvider;
        
        public BotService(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _discord = new DiscordClient(new DiscordConfiguration
            {
                Token = configuration["Bot_Key"],
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents | DiscordIntents.Guilds
            });
            _serviceProvider = serviceProvider;
        }
        
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var generalChannels= new List<ulong>();
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
                Services = _serviceProvider
            });

            commands.RegisterCommands<FetchCommandModule>();
            commands.RegisterCommands<AdminCommandModule>();

            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                
                var timeUtc = DateTime.UtcNow;
                var easternZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
                var easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
                var result = await Utility.GetResult($"{Endpoints.Schedule}?teamId=12&date={easternTime:yyyy-MM-dd}");

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
