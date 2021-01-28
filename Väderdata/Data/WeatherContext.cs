using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Väderdata.Data
{
    public class WeatherContext : DbContext
    {

        public DbSet<Weather> WeatherData { get; set; }
        public DbSet<WeatherView> WeatherViewData { get; set; }

        public WeatherContext(DbContextOptions<WeatherContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Weather>()
                .HasOne(p => p.WeatherView)
                .WithMany(b => b.WeatherList);
        }


    }
}
