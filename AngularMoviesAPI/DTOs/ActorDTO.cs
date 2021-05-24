using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.DTOs
{
    // Used to get response(including actor) from web api
    public class ActorDTO
    {
        public string name { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string biography { get; set; }
        public string picture { get; set; }
        public int id { get; set; }
    }
}
