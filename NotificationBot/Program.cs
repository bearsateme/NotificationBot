using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Microsoft.Extensions.Configuration;
using NotificationBot.Commands;
using NotificationBot.Models.Enums;

namespace NotificationBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.Development.json", optional: true);

            IConfiguration config = builder.Build();
            
            var discord = new DiscordClient(new DiscordConfiguration
            {
                Token = config["Bot_Key"],
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            });
            
            int numguilds = discord.Guilds.Count;
            
            int numchannels = 0;
                        
            //DiscordGuild[] ArrayGuilds;
            DiscordChannel[] ArrayChannels;

            var guilds = discord.Guilds.Values;
            var generalChannels = new List<ulong>();

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

            var commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { "!" },
            });
            
            commands.RegisterCommands<FetchCommandModule>();
            
            await discord.ConnectAsync();
            

            while (true)
            {
                var result = await Utility.GetResult($"https://statsapi.web.nhl.com/api/v1/schedule?teamId=12&date={DateTime.Today:yyyy-MM-dd}");

                if (result.Status is GameStatus.Away or GameStatus.None)
                {
                    await Task.Delay(86400000);
                    continue;
                }

                if (result.Status != GameStatus.Finished)
                {
                    await Task.Delay(300000);
                    continue;
                }

                if (result.HomeScore > result.AwayScore)
                {
                    foreach (var channel in generalChannels)
                    {
                        var generalChannel = await discord.GetChannelAsync(channel);
                        await generalChannel.SendMessageAsync("get your shit");    
                    }
                    
                    await Task.Delay(86400000);
                    continue;
                }

                foreach (var channel in generalChannels)
                {
                    var generalChannel = await discord.GetChannelAsync(channel);
                    await generalChannel.SendMessageAsync("get your shit");    
                }
                
                await Task.Delay(86400000);
            }
        }

       
    }
}