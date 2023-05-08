using BusinessLogic.Interfaces;
using Models;
using Models.Enums;
using Models.Json.LiveGame;
using Models.Json.Schedule;
using Models.ViewModels;
using Newtonsoft.Json;

namespace BusinessLogic.Managers
{
    public class UtilityManager : IUtilityManager
    {
        public IHttpClientFactory HttpClientFactory { get; set; }

        public async Task<LiveGameInfo> GetGameInfo(string path)
        {
            var httpClient = HttpClientFactory.CreateClient();
            var result = await httpClient.GetAsync(path);
            var response = await result.Content.ReadAsStringAsync();
            
            var schedule = JsonConvert.DeserializeObject<LiveGameBaseModel>(response);

            return new LiveGameInfo()
            {
                CurrentPeriod = schedule.LiveData.Linescore.CurrentPeriod,
                TimeLeft = schedule.LiveData.Linescore.CurrentPeriodTimeRemaining
            };
        }
        
        public async Task<Result> GetResult(int teamId)
        {
            var httpClient = HttpClientFactory.CreateClient();
            var timeUtc = DateTime.UtcNow;
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
            var easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

            var result = await httpClient.GetAsync($"{Endpoints.Schedule}?teamId={teamId}&date={easternTime:yyyy-MM-dd}");
            var response = await result.Content.ReadAsStringAsync();
            var schedule = JsonConvert.DeserializeObject<ScheduleBaseModel>(response);

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
