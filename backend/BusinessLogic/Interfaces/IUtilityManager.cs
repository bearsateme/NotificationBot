using Models.Json.LiveGame;
using Models.ViewModels;

namespace BusinessLogic.Interfaces
{
    public interface IUtilityManager
    {
        Task<LiveGameInfo> GetGameInfo(string path);
        Task<Result> GetResult(int teamId);
    }
}
