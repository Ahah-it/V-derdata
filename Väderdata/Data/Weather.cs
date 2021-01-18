using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Väderdata.Data
{
    public class Weather
    {

        public int Id { get; private set; }

        public DateTime date { get; set; }
        public string plats { get; set; }
        public float temperature { get; set; }
        public int humidity { get; set; }
    }
}
