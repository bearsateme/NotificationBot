using NotificationBot.Models.Enums;

namespace NotificationBot.Models
{
    public class Result
    {
        public GameStatus Status { get; set; }
        public string? Opponent { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public string Link { get; set; }
    }
}

