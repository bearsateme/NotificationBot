using Models.Enums;
using Models.Json;
using Models.ViewModels;
using Newtonsoft.Json;

namespace WhoGivesACluck
{
public class Utility
{
    private static HttpClient HttpClient { get; set; } = new HttpClient();

    public static async Task<LiveGameInfo> GetGameInfo(string path)
    {
        var result = await HttpClient.GetAsync(path);
        var response = await result.Content.ReadAsStringAsync();
        
        var schedule = JsonConvert.DeserializeObject<LiveGameModel>(response);

        return new LiveGameInfo()
        {
            CurrentPeriod = schedule.LiveData.Linescore.CurrentPeriod,
            TimeLeft = schedule.LiveData.Linescore.CurrentPeriodTimeRemaining
        };
    }
    public static async Task<Result> GetResult(string path)
    {
        var result = await HttpClient.GetAsync(path);
        var response = await result.Content.ReadAsStringAsync();
        Console.WriteLine(path);
        Console.WriteLine(response);
        var schedule = JsonConvert.DeserializeObject<ScheduleModel>(response);

        if (schedule is not { Dates.Count: > 0 })
        {
            return new Result()
            {
                Status = GameStatus.None
            };
        }

        var game = schedule.Dates.SelectMany(x => x.Games).ToList().FirstOrDefault();

        if (game == null)
        {
            return new Result()
            {
                Status = GameStatus.None
            };
        }

        if (game.Teams.Home.Team.Id != 12)
        {
            return new Result()
            {
                Status = GameStatus.Away
            };
        }
        
        switch (game.Status.DetailedState)
        {
            case "Scheduled":
                return new Result()
                {
                    Status = GameStatus.Pending,
                    Opponent = game.Teams.Away.Team.Name
                };
            case "In Progress":
            case "In Progress - Critical":
                return new Result()
                {
                    Status = GameStatus.Pending,
                    Opponent = game.Teams.Away.Team.Name,
                    AwayScore = game.Teams.Away.Score,
                    HomeScore = game.Teams.Home.Score,
                    Link = game.Link
                };
            case "Final":
                return new Result()
                {
                    Status = GameStatus.Finished,
                    Opponent = game.Teams.Away.Team.Name,
                    AwayScore = game.Teams.Away.Score,
                    HomeScore = game.Teams.Home.Score
                }; 
            default:
                return new Result()
                {
                    Status = GameStatus.Invalid
                };
        }
    }
}    
}

