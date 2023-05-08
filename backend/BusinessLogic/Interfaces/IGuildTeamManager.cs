using Models.Json.Team;

namespace BusinessLogic.Interfaces
{
    public interface IGuildTeamManager
    {
        Task<TeamsBaseModel> GetTeams();
    }
}
