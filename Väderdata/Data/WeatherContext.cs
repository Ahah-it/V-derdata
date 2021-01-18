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

        public DbSet<Weather> weatherData { get; set; }

        public WeatherContext(DbContextOptions<WeatherContext> options) : base(options)
        {
        }
    }
}
