using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gismeteo_weather.Models;
using Gismeteo_weather.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gismeteo_weather.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigin")]
    public class WeathersController : Controller
    {
        private readonly DBContext _context;

        public WeathersController(DBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// возвращение данных о погоде
        /// </summary>
        /// <param name="r">код города</param>
        /// <param name="id">идентификатор необходимый для фронта (значение некритично)</param>
        /// <returns></returns>
        [Route("parse/{r}/{id?}")]
        [HttpGet]
        public ActionResult<IEnumerable<Weather>> ParseWeatherByCity(string r,int id)
        {
            List<Weather> weathers = new List<Weather>();
            weathers = GisParse.GetWeathers("/"+r+"/",id);

            return weathers;
        }

        /// <summary>
        /// парсинг информации о погоде и заполнение в БД, при наличии данных обновление данных в БД
        /// </summary>
        /// <param name="id">код города</param>
        /// <returns>информация о погода из БД. Список (код города, температура, ветер, давление, влажность, г/м поле)</returns>
        [Route("update/{id?}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Weather>>> InsertOrUpdateWeatherByCity(int id)
        {
            List<Weather> weathers = new List<Weather>();
            var cityHref = await _context.Cities.FindAsync(id);
            if (cityHref != null)
            {
                weathers = GisParse.GetWeathers(cityHref.Reference, id);
                if (weathers.Any())
                {
                    foreach(var weather in weathers)
                    {
                        var dataForUpdate = await _context.Weathers.FirstOrDefaultAsync(x => x.CityID.Equals(weather.CityID) && x.Date.Equals(weather.Date) && x.Timeofday.Equals(weather.Timeofday));
                        if (dataForUpdate != null)
                        {
                            dataForUpdate.Temp = weather.Temp;
                            dataForUpdate.Geomagneticf = weather.Geomagneticf;
                            dataForUpdate.Windspeed = weather.Windspeed;
                            dataForUpdate.Pressure = weather.Pressure;
                            dataForUpdate.Humidity = weather.Humidity;
                        } else
                        {
                            _context.Weathers.Add(weather);
                        }
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return weathers;
        }
    }
}