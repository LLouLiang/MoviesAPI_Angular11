using AngularMoviesAPI.helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.DTOs
{
    public class MovieCreationDTO
    {
        public string title { get; set; }
        public string summary { get; set; }
        public string trailer { get; set; }
        public bool inTheater { get; set; }
        public DateTime releaseDate { get; set; }
        public IFormFile poster { get; set; }

        [ModelBinder(BinderType = typeof(customModelBinder<List<int>>))]
        public List<int> genreIds { get; set; }
        [ModelBinder(BinderType = typeof(customModelBinder<List<int>>))]
        public List<int> movietheaterIds { get; set; }
        [ModelBinder(BinderType = typeof(customModelBinder<List<MovieActorsCreationDTO>>))]
        public List<MovieActorsCreationDTO> movieActors { get; set; }
    }
}
