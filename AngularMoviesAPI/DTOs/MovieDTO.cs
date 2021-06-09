using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.DTOs
{
    public class MovieDTO
    {
        public string title { get; set; }
        public string summary { get; set; }
        public string trailer { get; set; }
        public bool inTheater { get; set; }
        public DateTime releaseDate { get; set; }
        public string poster { get; set; }
        public int id { get; set; }
        
        public List<MovieTheaterDTO> movieTheaters { get; set; }
        public List<GenreDTO> genres { get; set; }
        public List<ActorsMovieDTO> actors { get; set; }
    }
}
