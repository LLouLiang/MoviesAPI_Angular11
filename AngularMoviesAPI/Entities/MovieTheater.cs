using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.Entities
{
    public class MovieTheater
    {
        [Required]
        [StringLength(75)]
        public string name { get; set; }
        public int id { get; set; }
        public Point location { get; set; }
    }
}
