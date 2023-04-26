using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using NotificationBot.Models.Enums;

namespace NotificationBot.Commands
{
    public class FetchCommandModule : BaseCommandModule 
    {
        [Command("status")]
        public async Task FetchCommand(CommandContext context)
        {
            var result = Utility.GetResult($"https://statsapi.web.nhl.com/api/v1/schedule?teamId=12&date={DateTime.Today:yyyy-MM-dd}");

            switch (result.Result.Status)
            {
                case GameStatus.Away:
                    await context.RespondAsync("Hurricanes are playing away.");
                    break;
                case GameStatus.Invalid:
                case GameStatus.None:
                    await context.RespondAsync("Hurricanes are not playing today");
                    break;
                case GameStatus.Pending:
                    await context.RespondAsync("The game has not yet finished");
                    await context.RespondAsync($"Hurricanes: {result.Result.HomeScore}");
                    await context.RespondAsync($"{result.Result.Opponent}: {result.Result.AwayScore}");
                    break;
                case GameStatus.Finished:
                    await context.RespondAsync("The game has finished");
                    await context.RespondAsync($"Hurricanes: {result.Result.HomeScore}");
                    await context.RespondAsync($"{result.Result.Opponent}: {result.Result.AwayScore}");
                    if (result.Result.HomeScore > result.Result.AwayScore)
                    {
                        await context.RespondAsync("Have you gotten your free sandwich?");
                    }
                    else
                    {
                        await context.RespondAsync("No sandwiches unfortunately");
                    }
                    break;
            }
        }
    
    }
}
