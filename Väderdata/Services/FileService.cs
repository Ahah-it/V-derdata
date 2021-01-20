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
    public class FileService
    {
        WeatherContext context;
        IWebHostEnvironment env;
        public FileService(WeatherContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }

        public int ReadFileIntoDatabase()
        {
            //Open file.
            var path = $"{ env.WebRootPath }\\db.csv";
            using (var reader = new StreamReader(File.OpenRead(path)))
            {
                string line;

                //Format provider to parse decimal numbers.
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                provider.NumberGroupSeparator = "";

                List<Weather> weatherList = new List<Weather>();

                int counter = 0;

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

                //Add & Save changes to database.
                Task t = context.AddRangeAsync(weatherList);
                t.Wait();
                context.SaveChanges();
                weatherList.Clear();


                return counter;
            }
        }


    }
}
