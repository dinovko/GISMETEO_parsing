using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gismeteo_weather.Models
{
    public class DBContext:DbContext
    {
        public DBContext(DbContextOptions<DBContext> options):base(options)
        {

        }

        public DbSet<Weather> Weathers { get; set; }
        public DbSet<City> Cities { get; set; }
    }
}
