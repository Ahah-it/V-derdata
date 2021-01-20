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

        public WeatherContext(DbContextOptions<WeatherContext> options) : base(options)
        {
            
        }

        public List<Weather> GetRecordsFromDate(DateTime date)
        {
            return WeatherData
                .Where((m) => m.Date.Date.Equals(date.Date))
                .ToList();
        }


    }
}
