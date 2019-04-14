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
    public class CitiesController : Controller
    {
        private readonly DBContext _context;
        public CitiesController(DBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Возвращает список городов
        /// </summary>
        /// <returns>список (ИД, код города, название города)</returns>
        [Route("list")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCities()
        {
            var data = await _context.Cities.ToListAsync();
            return data;
        }

        /// <summary>
        /// Парсит, возвращает список городов
        /// </summary>
        /// <returns>список (ИД, код города, название города)</returns>
        [Route("parse/list")]
        [HttpGet]
        public ActionResult<IEnumerable<City>> ParseCities()
        {
            var cities = GisParse.GetCitiesFromURL();
            return cities;
        }

        /// <summary>
        /// Парсит. Заполняет и обновляет список городов в БД
        /// возвращает данные из БД
        /// </summary>
        /// <returns>список (ИД, код города, название города)</returns>
        [Route("update")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> InsertOrUpdateCity()
        {
            List<City> cities = new List<City>();
            cities = GisParse.GetCitiesFromURL();
            if (cities.Any())
            {
                foreach(var city in cities)
                {
                    var dataForUpdate = await _context.Cities.FirstOrDefaultAsync(x => x.Reference.Equals(city.Reference));
                    if(dataForUpdate == null)
                    {
                        _context.Cities.Add(
                            new City
                            {
                                CityName = city.CityName,
                                Reference = city.Reference
                            }
                            );
                    }
                    await _context.SaveChangesAsync();
                }
            }

            var citiesFromDB = await _context.Cities.ToListAsync();

            return citiesFromDB;
        }

    }
}