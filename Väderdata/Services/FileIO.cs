using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Väderdata.Data;

namespace Väderdata.Services
{
    public class FileIO
    {
        WeatherContext context;
        IWebHostEnvironment env;
        WeatherQueries query;
        public FileIO(WeatherContext context, IWebHostEnvironment env, WeatherQueries query)
        {
            this.context = context;
            this.env = env;
            this.query = query;
        }

        public int ReadFileIntoDatabase()
        {
            //File path.
            var path = $"{ env.WebRootPath }\\db.csv";
            List<Weather> weatherList = new List<Weather>();
            int counter = 0;

            counter = ParseCSV(path, weatherList, counter);

            //Add & Save changes to database.
            Task t1 = context.AddRangeAsync(weatherList);
            t1.Wait();
            context.SaveChanges();

            //Create Daily views for every date.
            var uniqueDates = weatherList.GroupBy(x => x.Date.Date).ToHashSet();
            List<WeatherView> weatherViewList = new List<WeatherView>();
            CreateDailyView(weatherList, uniqueDates, weatherViewList);
                
            //Add & Save changes to database.
            Task t2 = context.AddRangeAsync(weatherViewList);
            t2.Wait();
            context.SaveChanges();

            weatherViewList.Clear();
            weatherList.Clear();

            query.SetSummerSeason();
            query.SetAutumnSeason();
            query.SetWinterSeason();
            context.SaveChanges();

            return counter;

        }

        private static void CreateDailyView(List<Weather> weatherList, HashSet<IGrouping<DateTime, Weather>> uniqueDates, List<WeatherView> weatherViewList)
        {

            Parallel.ForEach(uniqueDates, (date) =>
            {
                WeatherView indoorView = new WeatherView();
                WeatherView outdoorView = new WeatherView();

                indoorView.Date = date.Key;
                indoorView.Location = "Inne";
                indoorView.WeatherList = weatherList.Where(d => d.Date.Date.Equals(date.Key) && d.Location.Contains("Inne")).ToList();

                outdoorView.Date = date.Key;
                outdoorView.Location = "Ute";
                outdoorView.WeatherList = weatherList.Where(d => d.Date.Date.Equals(date.Key) && d.Location.Contains("Ute")).ToList();


                //Calculate Average and mould risk
                indoorView.AverageHumidity = indoorView.WeatherList.Average(t => t.Humidity);
                indoorView.AverageTemperature = indoorView.WeatherList.Average(t => t.Temperature);
                indoorView.MouldRisk = CalculateMouldRisk(indoorView);

                outdoorView.AverageHumidity = outdoorView.WeatherList.Average(t => t.Humidity);
                outdoorView.AverageTemperature = outdoorView.WeatherList.Average(t => t.Temperature);
                outdoorView.MouldRisk = CalculateMouldRisk(outdoorView);


                weatherViewList.Add(indoorView);
                weatherViewList.Add(outdoorView);
            });
        }

        private static double CalculateMouldRisk(WeatherView view)
        {
            return (view.AverageHumidity - 78) * (view.AverageTemperature / 15) / 0.22;
        }

        private static int ParseCSV(string path, List<Weather> weatherList, int counter)
        {
            using (var reader = new StreamReader(File.OpenRead(path)))
            {
                string line;

                //Format provider to parse decimal numbers.
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                provider.NumberGroupSeparator = "";

                //Parse csv records and add them to list
                while ((line = reader.ReadLine()) != null)
                {
                    Weather weather = new Weather();
                    string[] arr = line.Split(',');
                    try
                    {
                        weather.Date = Convert.ToDateTime(arr[0]);
                        weather.Location = arr[1];
                        weather.Temperature = (float)Convert.ToDouble(arr[2], provider);
                        weather.Humidity = Convert.ToInt32(arr[3]);

                        //Add csv record to list.
                        weatherList.Add(weather);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    counter++;

                }

            }

            return counter;
        }
    }
}
