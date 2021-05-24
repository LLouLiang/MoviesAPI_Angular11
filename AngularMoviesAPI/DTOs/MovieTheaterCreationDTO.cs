using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.DTOs
{
    public class MovieTheaterCreationDTO
    {
        public string name { get; set; }
        [Range(-90, 90)]
        public double latitude { get; set; }
        [Range(-180, 200)]
        public double longitude { get; set; }
    }
}
