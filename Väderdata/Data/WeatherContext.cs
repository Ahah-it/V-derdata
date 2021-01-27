using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;

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

        //Get all records from the day
        public List<Weather> GetRecordsFromDate(WeatherView view)
        {
            return WeatherData
                .Where((m) => m.Date.Date.Equals(view.Date.Date))
                .Where((p) => p.Location == (view.Location))
                .ToList();
        }

        public WeatherView GetStatFromDate(WeatherView view)
        {
            //Look up record if it exists already.
            var query = WeatherViewData.Where(d => (d.Date == view.Date));
            if(query.Any())
            {
                if(query.First().Location.Contains(view.Location))
                    return query.First();
            }


            return view;
        }
        
       

    }
}
