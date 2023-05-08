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

            await context.RespondAsync("DI test");
        }
    }
}
