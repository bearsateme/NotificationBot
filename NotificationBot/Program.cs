using DSharpPlus;
using DSharpPlus.CommandsNext;
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
                .AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();
            
            var discord = new DiscordClient(new DiscordConfiguration
            {
                Token = config["Bot_Key"],
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            });

            var generalChannel = await discord.GetChannelAsync(ulong.Parse(config["Channel_Id"]));
            
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
                    await generalChannel.SendMessageAsync("get your shit");
                    await Task.Delay(86400000);
                    continue;
                }

                await generalChannel.SendMessageAsync("fuck we lost at home");
                await Task.Delay(86400000);
            }
        }

       
    }
}