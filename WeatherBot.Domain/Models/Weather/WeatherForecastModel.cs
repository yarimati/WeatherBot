namespace WeatherBot.Domain.Models.Weather
{
    public class WeatherForecastModel
    {
        public WeatherDescriptionModel[] weather { get; set; }
        public string _base { get; set; }
        public WeatherModel main { get; set; }
        public int visibility { get; set; }
        public int dt { get; set; }
        public int timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }
}