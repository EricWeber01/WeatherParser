using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Weather w = new Weather();
            //w.GetWeather(4);
            w.ShowForecast("Варшава");
            w.ShowForecast("Аляска");
            w.ShowMostHot();
        }
    }
}
