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

        public List<WeatherView> GetTemperautreAverageWithinBounds(double min, double max)
        {
            //Returns all dates with temperature between min and max
            return db.WeatherViewData
                .Where(temp => temp.AverageTemperature > min)
                .Where(temp => temp.AverageTemperature < max)
                .ToList();
        }

        public DateTime GetFirstDateWithConsecutiveDays(List<WeatherView> list, int numberOfConsecutiveDays)
        {
            var orderedList = list.OrderBy(x => x.Date).Where(x => x.Location.Contains("Ute"));
            DateTime previousDate = new DateTime();
            DateTime currentDate = new DateTime();
            int counter = 0;

            previousDate = orderedList.First().Date;
            currentDate = orderedList.First().Date;

            foreach (WeatherView cons in orderedList)
            {
                previousDate = cons.Date.AddDays(-1);
                if(previousDate.Date.Date.Equals(currentDate.Date)) {
                    counter++;
                    currentDate = cons.Date;
                    if(counter >= numberOfConsecutiveDays)
                    {
                        return currentDate.AddDays(-numberOfConsecutiveDays);
                    }
                } else
                {
                    counter = 0;
                    currentDate = cons.Date;
                }
            }

            return new DateTime();
        }

        public WeatherView GetAutumnStartDate()
        {
            //5 consecutive days with temperatures below 10 and above 0 degrees is autumn season.
            List<WeatherView> validDates = GetTemperautreAverageWithinBounds(0, 10);
            DateTime startDate = GetFirstDateWithConsecutiveDays(validDates, 5);

            if (startDate != default(DateTime))
            {
                return db.WeatherViewData
                    .Where(x => x.Date.Date.Equals(startDate))
                    .Where(l => l.Location.Contains("Ute"))
                    .First();
            }

            return default(WeatherView);
        }
        // Find winter Start Date.
        public WeatherView GetWinterStartDate() {
            //5 consecutive days with temperatures below 0 degrees is winter season and before 31 july.
            List<WeatherView> validDates = GetTemperautreAverageWithinBounds(Double.MinValue, 0);
            DateTime startDate = GetFirstDateWithConsecutiveDays(validDates, 5);
            if (startDate != default(DateTime))
            {
                return db.WeatherViewData.Where(x => x.Date.Date.Equals(startDate)).First();
            }
            return default(WeatherView);
        }

        //Get stats for one date.
        public WeatherView GetStatFromDate(WeatherView view)
        {
            var query = db.WeatherViewData
                .Where(d => (d.Date == view.Date))
                .Where((p) => p.Location.Contains(view.Location));
            if (query.Any())
            {
                return query.First();
            }
            return default;
        }

        //Get all records from the day
        public List<Weather> GetRecordsFromDate(WeatherView view)
        {
            List<Weather> query = db.WeatherData
                    .Where((m) => m.Date.Date.Equals(view.Date.Date))
                    .Where((p) => p.Location.Contains(view.Location))
                    .ToList();

            if (query.Any())
            {
                return query;
            }
            else
            {
                return default;
            }
        }
    }
}
