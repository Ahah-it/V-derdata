using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Väderdata.Data
{
    public class WeatherView
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double AverageTemperature { get; set; }
        public double AverageHumidity { get; set; }
        public double MouldRisk { get; set; }
        public string Location { get; set; }

        public List<Weather> WeatherList { get; set; }
    }
}
