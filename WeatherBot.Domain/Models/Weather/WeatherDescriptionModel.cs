﻿namespace WeatherBot.Domain.Models.Weather
{
    public class WeatherDescriptionModel
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }
}