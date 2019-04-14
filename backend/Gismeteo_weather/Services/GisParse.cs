using Gismeteo_weather.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gismeteo_weather.Services
{
    public static class GisParse
    {
        /// <summary>
        /// загружает данные с сайта
        /// парсит
        /// </summary>
        /// <returns>список (ИД, идентификатор города, название города)</returns>
        public static List<City> GetCitiesFromURL()
        {
            var request = "https://www.gismeteo.kz/catalog/kazakhstan/";
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(request);
            HtmlNodeCollection cityList = htmlDoc.DocumentNode.SelectNodes("//section[@class=\"catalog_block\"]/div[@class=\"catalog_list catalog_list_ordered\"]/div[@class=\"catalog_item\"]/a");
            List<City> cities = new List<City>();
            int cityCount = 0;
            foreach (var city in cityList)
            {
                var cityHref = city.Attributes["href"].Value.Trim();
                if (cityHref.Contains("/weather"))
                {
                    string cityName = city.InnerText.Trim();
                    while (!cities.Any(r => r.Reference.Equals(cityHref)))
                    {
                        cities.Add(
                            new City
                            {
                                Id = cityCount,
                                CityName = cityName,
                                Reference = cityHref
                            }
                        );
                        cityCount++;
                    }
                }

            }
            return cities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<City> GetCities()
        {
            var request = "https://www.gismeteo.kz/catalog/kazakhstan/";
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(request);
            int cityCount = 0;
            HtmlNodeCollection par0 = htmlDoc.DocumentNode.SelectNodes("//div[@class=\"catalog_sides\"]/div[@class=\"catalog_side\"]/section[@class=\"catalog_block\"]/div[@class=\"catalog_list catalog_list_ordered\"]/div[@class=\"catalog_item\"]/a");
            HtmlNodeCollection cityList = htmlDoc.DocumentNode.SelectNodes("//section[@class=\"catalog_block\"]/div[@class=\"catalog_list catalog_list_ordered\"]/div[@class=\"catalog_item\"]/a");
            List<City> cities = new List<City>();
            foreach (var item in cityList)
            {
                var cityHref = item.Attributes["href"].Value.Trim();
                if (cityHref.Contains("/weather"))
                {
                    string item_2 = item.InnerText.Trim();

                    if (!cities.Any(c => c.Reference.Equals(cityHref)))
                    {
                        cities.Add(
                       new City { CityName = item_2, Reference = cityHref }
                       );
                        cityCount++;
                    }

                }
            }

            return cities;
        }

        /// <summary>
        /// Парсинг
        /// получение данных о погоде по уникальной ссылке города
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="id"></param>
        /// <returns>список (код города, температура, ветер, давление, влажность, г/м поле)</returns>
        public static List<Weather> GetWeathers(string reference, int id)
        {
            List<Weather> weathers = new List<Weather>(13);
            var url = "https://www.gismeteo.kz" + reference + "3-days/";
            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(url);

            HtmlNodeCollection timeOfDayNode = htmlDoc.DocumentNode.SelectNodes("//div[@class=\"_line timeline nil clearfix\"]/div");
            HtmlNodeCollection temperaturesNode = htmlDoc.DocumentNode.SelectNodes("//div[@class=\"chart chart__temperatureByDay\"]/div[@class=\"values\"]/div");
            HtmlNodeCollection dateNode = htmlDoc.DocumentNode.SelectNodes("//div[@class=\"header_item js_head_item frame_4 showed clearfix\"]/a");
            HtmlNodeCollection windNode = htmlDoc.DocumentNode.SelectNodes("//div[@class=\"_line windline js_wind clearfix\"]/div");
            HtmlNodeCollection pressureNode = htmlDoc.DocumentNode.SelectNodes("//div[@class=\"widget__row widget__row_pressure\"]/div[@class=\"js_pressure pressureline w_pressure\"]/div[@class=\"chart chart__pressure\"]/div[@class=\"values\"]/div");
            HtmlNodeCollection humidityNode = htmlDoc.DocumentNode.SelectNodes("//div[@class=\"widget__row widget__row_table widget__row_humidity\"]/div");
            HtmlNodeCollection geomagneticNode = htmlDoc.DocumentNode.SelectNodes("//div[@class=\"widget__container\"]/div[@class=\"widget__row widget__row_table widget__row_gm\"]/div");



            for (int i = 0; i < 12; i++)
            {
                weathers.Add(
                    new Weather
                    {
                        CityID = id,
                        Date = StrToDate(dateNode[0].InnerHtml, i),
                        Timeofday = timeOfDayNode[i].InnerText.ToString(),
                        Temp = Convert.ToInt16(temperaturesNode[i].FirstChild.InnerText.Replace("&minus;", "-")),
                        Windspeed = StrToWind(windNode[i], i),
                        Pressure = Convert.ToInt16(pressureNode[i].FirstChild.InnerHtml.Trim()),
                        Humidity = Convert.ToInt16(humidityNode[i].FirstChild.InnerHtml.Trim()),
                        Geomagneticf = Convert.ToInt16(geomagneticNode[i].FirstChild.InnerHtml.Trim())
                    });

            }

            return weathers;
        }

        /// <summary>
        /// преобразование строки в дату
        /// </summary>
        /// <param name="s"></param>
        /// <param name="i">идентификатор для подсчитывания времени суток и количества дней</param>
        /// <returns></returns>
        public static DateTime StrToDate(string s, int i)
        {
            string[] mounth = { "янв", "февр", "март", "апр", "май", "июнь", "июль", "авг", "сен", "окт", "нояб", "дек" };
            string[] dates = s.Split(" ");
            int M = Array.FindIndex(mounth, x => x.Equals(dates[2])) + 1;
            DateTime dateTime = new DateTime(2019, M, Convert.ToInt16(dates[1]));
            String shortDate = dateTime.AddDays((int)(i / 4)).ToShortDateString();

            return Convert.ToDateTime(shortDate);
        }

        /// <summary>
        /// преобразование строки в скорость ветра
        /// </summary>
        /// <param name="w"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int StrToWind(HtmlNode w, int item)
        {
            HtmlNodeCollection windSpeed = w.SelectNodes("//span[@class=\"unit unit_wind_m_s\"]");

            return Convert.ToInt16(windSpeed[item].InnerHtml.Trim());
        }
    }
}
