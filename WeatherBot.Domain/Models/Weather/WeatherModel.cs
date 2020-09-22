﻿namespace WeatherBot.Domain.Models.Weather
{
    public class WeatherModel
    {
        public float temp { get; set; }
        public float feels_like { get; set; }
        public int temp_min { get; set; }
        public float temp_max { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
    }
}