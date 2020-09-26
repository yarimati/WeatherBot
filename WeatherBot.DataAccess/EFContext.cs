using Microsoft.EntityFrameworkCore;
using WeatherBot.DataAccess.Models;
using WeatherBot.Domain;

namespace WeatherBot.DataAccess
{
    public class EFContext : DbContext
    {
        public DbSet<UserSchedule> UsersSchedule { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ApiKeys.ConnectionString);
        }
    }
}