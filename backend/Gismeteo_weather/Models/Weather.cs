using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gismeteo_weather.Models
{
    public class Weather
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int? CityID { get; set; }

        [Required]
        public City City { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(-100,100)]
        public int Temp { get; set; }

        public double Precipitation { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Timeofday { get; set; }

        [Required]
        [Range(0, 200)]
        public int Windspeed { get; set; }

        [StringLength(50, MinimumLength =1)]
        public string Winddirection { get; set; }

        [Range(100, 950)]
        public int Pressure { get; set; }

        [Range(0, 100)]
        public int Humidity { get; set; }

        [Range(0, 10)]
        public int Geomagneticf { get; set; }

    }
}
