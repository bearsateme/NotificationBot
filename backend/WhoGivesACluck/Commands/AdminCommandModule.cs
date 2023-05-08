using System.Text;
using BusinessLogic.Interfaces;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace WhoGivesACluck.Commands
{
    public class AdminCommandModule : BaseCommandModule
    {
        public IGuildTeamManager GuildTeamManager { get; set; }

        public AdminCommandModule(IGuildTeamManager guildTeamManager)
        {
            GuildTeamManager = guildTeamManager;
        }
        
        [Command("allteams")]
        public async Task ListTeams(CommandContext context)
        {
            var teamList = await GuildTeamManager.GetTeams();

            var builder = new StringBuilder().Append("Teams:").AppendLine();

            foreach (var team in teamList.Teams)
            {
                builder.Append($"{team.Id}: {team.Name}").AppendLine();
            }

            await context.RespondAsync(builder.ToString());
        }
    }
}
