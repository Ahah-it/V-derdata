using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Väderdata.Data
{
    public class WeatherQueries
    {
        WeatherContext db;
        public WeatherQueries(WeatherContext context)
        {
            db = context;
        }

        //Highest to lowest temp/humidity/date/mouldrisk

        public List<WeatherView> GetViewOrderedByHumidity(bool descending, bool indoor)
        {
            if (!descending)
            {
                return db.WeatherViewData
                    .OrderBy(x => x.AverageHumidity)
                    .Where(l => l.Location.Contains( indoor ? "Inne" : "Ute"))
                    .ToList();
            }
            else
            {
                return db.WeatherViewData
                    .OrderByDescending(x => x.AverageHumidity)
                    .Where(l => l.Location.Contains( indoor ? "Inne" : "Ute"))
                    .ToList();
            }
        }

        public List<WeatherView> GetViewOrderedByTemperature(bool descending, bool indoor)
        {
            if (!descending)
            {
                return db.WeatherViewData
                    .OrderBy(x => x.AverageTemperature)
                    .Where(l => l.Location.Contains(indoor ? "Inne" : "Ute"))
                    .ToList();
            }
            else
            {
                return db.WeatherViewData
                    .OrderByDescending(x => x.AverageTemperature)
                    .Where(l => l.Location.Contains(indoor ? "Inne" : "Ute"))
                    .ToList();
            }
        }

        public List<WeatherView> GetViewOrderedByMouldRisk(bool descending, bool indoor)
        {
            if (!descending)
            {
                return db.WeatherViewData
                    .OrderBy(x => x.MouldRisk)
                    .Where(l => l.Location.Contains(indoor ? "Inne" : "Ute"))
                    .ToList();
            }
            else
            {
                return db.WeatherViewData
                    .OrderByDescending(x => x.MouldRisk)
                    .Where(l => l.Location.Contains(indoor ? "Inne" : "Ute"))
                    .ToList();
            }
        }
    }
}
