using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.Entities
{
    public class MovieActors
    {
        public int actorId { get; set; }
        public int movieId { get; set; }
        public string character { get; set; }
        public int order { get; set; }
        public Actor actor { get; set; }
        public Movie movie { get; set; }
    }
}
