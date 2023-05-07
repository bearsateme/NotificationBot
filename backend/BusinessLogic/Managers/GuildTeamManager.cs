using BusinessLogic.Interfaces;

namespace BusinessLogic.Managers
{
    public class GuildTeamManager : IGuildTeamManager
    {
        public async Task<List<string>> GetTeams()
        {
            return new List<string>()
            {
                "test1"
            };
        }
    }
}
