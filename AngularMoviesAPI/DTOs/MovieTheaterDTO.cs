using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.DTOs
{
    public class MovieTheaterDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        [Range(-90,90)]
        public double latitude { get; set; }
        [Range(-180, 180)]
        public double longitude { get; set; }
    }
}
