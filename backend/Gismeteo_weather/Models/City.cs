using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gismeteo_weather.Models
{
    public class City
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(200,MinimumLength =2)]
        public string CityName { get; set; }

        [Required]
        [StringLength(254, MinimumLength =5)]
        public string Reference { get; set; }
    }
}
