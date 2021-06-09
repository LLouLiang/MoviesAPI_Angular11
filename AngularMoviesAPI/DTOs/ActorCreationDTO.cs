using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.DTOs
{
    // Creating a new Actor will be using this DTO object
    public class ActorCreationDTO
    {
        public string name { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string biography { get; set; }
        public IFormFile picture { get; set; }
    }
}
