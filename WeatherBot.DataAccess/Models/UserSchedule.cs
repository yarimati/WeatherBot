using System;

namespace WeatherBot.DataAccess.Models
{
    public class UserSchedule
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ChatId { get; set; }
        public TimeSpan ScheduledTime { get; set; }
    }
}