using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Text.RegularExpressions;
namespace WeatherParser
{
    class Weather
    {

        string[] city;
        string[] TownNames;
        public string Town { get; set; }
        public string Max { get; set; }
        public string Min { get; set; }
        XmlDocument doc;
        XmlElement root;
        Parser parser;
        public Weather()
        {
            string linq = "https://xml.meteoservice.ru/export/gismeteo/point/";
            TownNames = new string[] { "Киев", "Кривой Рог", "Париж", "Прага", "Варшава" };
            city = new string[] { linq + "25.xml", linq + "28.xml", linq + "247.xml", linq + "310.xml", linq + "294.xml" };
            doc = new XmlDocument();
            parser = new Parser();
        }
        public void GetWeather(int ind)
        {
            doc.Load(city[ind]);
            root = doc.DocumentElement;
            Town = TownNames[ind];
            foreach (XmlNode item in doc.GetElementsByTagName("TEMPERATURE"))
            {
                XmlNode max = item.Attributes.GetNamedItem("max");
                XmlNode min = item.Attributes.GetNamedItem("min");
                if (max != null && min != null)
                {
                    Max = max.Value;
                    Min = min.Value;
                    break;
                }
            }
            Console.WriteLine($"{Town} | Max: {Max}C` | Min {Min}C`");
            parser.WriteToXml(this);
        }
        public void ShowForecast(string str)
        {
            parser.ShowForecast(str);
        }
        public void ShowMostHot()
        {
            parser.ShowMostHot();
        }
    }
    class Parser
    {

        XmlDocument doc;
        XmlNode root;
        public Parser()
        {
            doc = new XmlDocument();
        }
        public void WriteToXml(Weather obj)
        {
            doc.Load("weather.xml");
            root = doc.DocumentElement;
            XmlElement city = doc.CreateElement("City");
            city.SetAttribute("city", obj.Town);
            XmlNode maxT = doc.CreateElement("MaxT");
            XmlNode minT = doc.CreateElement("MinT");
            XmlNode text1 = doc.CreateTextNode(obj.Max + " C`");
            XmlNode text2 = doc.CreateTextNode(obj.Min + " C`");
            maxT.AppendChild(text1);
            minT.AppendChild(text2);
            city.AppendChild(maxT);
            city.AppendChild(minT);
            root.AppendChild(city);
            doc.Save("weather.xml");
        }
        public void ShowForecast(string city)
        {
            doc.Load("weather.xml");
            root = doc.DocumentElement;

            foreach (XmlElement item in root)
            {
                if (item.Attributes["city"].Value == city)
                {
                    Console.WriteLine(city);
                    foreach (XmlNode itemchild in item.ChildNodes)
                    {
                        Console.WriteLine($"{itemchild.Name}: {itemchild.InnerText}");
                    }
                    break;
                }
            }
        }
        public void ShowMostHot()
        {
            doc.Load("weather.xml");
            root = doc.DocumentElement;
            string HotCity = "";
            int max = 0;
            foreach (XmlElement item in root)
                foreach (XmlNode itemchild in item.ChildNodes)
                    if (max < Convert.ToInt32(Regex.Replace(itemchild.InnerText, @"[^\d]+", "")))
                    {
                        HotCity = item.Attributes["city"].Value + " | " + itemchild.Name + ": " + itemchild.InnerText;
                        max = Convert.ToInt32(Regex.Replace(itemchild.InnerText, @"[^\d]+", ""));
                    }
            Console.WriteLine($"Самый теплый город\n{HotCity}");
        }
    }
}