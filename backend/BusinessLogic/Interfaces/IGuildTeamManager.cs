namespace BusinessLogic.Interfaces
{
    public interface IGuildTeamManager
    {
        Task<List<string>> GetTeams();
    }
}
