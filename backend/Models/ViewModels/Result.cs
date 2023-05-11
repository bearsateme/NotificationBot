﻿namespace Models.ViewModels
{
    public class Result
    {
        public string Status { get; set; }
        public string? Opponent { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public string Link { get; set; }
    }
}

