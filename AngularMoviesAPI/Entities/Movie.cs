using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.Entities
{
    public class Movie
    {
        public string title { get; set; }
        public string summary { get; set; }
        public string trailer { get; set; }
        public bool inTheater { get; set; }
        public DateTime releaseDate { get; set; }
        public string poster { get; set; }
        public int id { get; set; }
        public List<MovieGenres> movieGenres { get; set; }
        public List<MovieTheaterMovies> movieTheaterMovies { get; set; }
        public List<MovieActors> movieActors { get; set; }
    }
}
