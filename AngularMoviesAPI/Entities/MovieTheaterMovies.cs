using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.Entities
{
    public class MovieTheaterMovies
    {
        public int movieTheaterId { get; set; }
        public int movieId { get; set; }
        public MovieTheater movieTheater { get; set; }
        public Movie movie { get; set; }
    }
}
