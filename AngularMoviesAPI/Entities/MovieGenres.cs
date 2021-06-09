using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.Entities
{
    public class MovieGenres
    {
        public int genreId { get; set; }
        public int movieId { get; set; }

        public Genre Genre { get; set; }
        public Movie Movie { get; set; }
    }
}
