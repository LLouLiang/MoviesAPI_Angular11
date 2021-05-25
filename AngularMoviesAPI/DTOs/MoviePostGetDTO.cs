using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.DTOs
{
    public class MoviePostGetDTO
    {
        public List<MovieTheaterDTO> movieTheaters { get; set; }
        public List<GenreDTO> genres { get; set; }
    }
}
