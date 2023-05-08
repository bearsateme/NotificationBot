using System.Text;
using BusinessLogic.Interfaces;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Models.Enums;

namespace WhoGivesACluck.Commands
{
    public class FetchCommandModule : BaseCommandModule 
    {
        public IUtilityManager UtilityManager { get; set; }
        
        [Command("status")]
        public async Task FetchCommand(CommandContext context)
        {
            var result = await UtilityManager.GetResult(12);
            var builder = new StringBuilder();

            switch (result.Status)
            {
                case GameStatus.Away:
                    await context.RespondAsync("Hurricanes are playing away.");
                    break;
                case GameStatus.Invalid:
                case GameStatus.None:
                    await context.RespondAsync("Hurricanes are not playing today");
                    break;
                case GameStatus.Pending:
                    builder.AppendLine("The game has not yet finished");
                    builder.AppendLine($"Hurricanes: {result.HomeScore}");
                    builder.AppendLine($"{result.Opponent}: {result.AwayScore}");

                    var gamePath = $"https://statsapi.web.nhl.com{result.Link}";
                    var liveInfo = await UtilityManager.GetGameInfo(gamePath);

                    builder.AppendLine($"Period: {liveInfo.CurrentPeriod}");
                    builder.AppendLine($"Time Left: {liveInfo.TimeLeft}");
                    
                    await context.RespondAsync(builder.ToString());
                    break;
                case GameStatus.Finished:
                    builder.AppendLine("The game has finished");
                    builder.AppendLine($"Hurricanes: {result.HomeScore}");
                    builder.AppendLine($"{result.Opponent}: {result.AwayScore}");
                    if (result.HomeScore > result.AwayScore)
                    {
                        builder.AppendLine("Have you gotten your free sandwich?");
                    }
                    else
                    {
                        builder.AppendLine("No sandwiches unfortunately");
                    }
                    await context.RespondAsync(builder.ToString());
                    break;
            }
        }
    
    }
}
