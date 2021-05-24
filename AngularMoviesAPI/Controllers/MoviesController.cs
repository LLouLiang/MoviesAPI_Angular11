using AngularMoviesAPI.DTOs;
using AngularMoviesAPI.Entities;
using AngularMoviesAPI.helpers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.Controllers
{
    [Route("movie/movies")]
    public class MoviesController : ControllerBase
    {
        // Services injection
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;
        private readonly AzureStorageService storageService;
        private string container = "Movies";

        public MoviesController(IMapper mapper, ApplicationDbContext context, AzureStorageService storageService)
        {
            this.mapper = mapper;
            this.context = context;
            this.storageService = storageService;
        }
        //[httpget]
        //public async task<actionresult<list<moviedto>>> get()
        //{
        //    return nocontent();
        //}
        //[httpget("{id:int}")]
        //public async task<actionresult<moviedto>> get(int id)
        //{
        //    return nocontent();
        //}
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] MovieCreationDTO movietheaterDTO)
        {
            var movie = mapper.Map<Movie>(movietheaterDTO);
            if(movietheaterDTO.poster != null)
            {
                movie.poster = await storageService.saveFile(container, movietheaterDTO.poster);
            }
            annotateActorsOrder(movie);
            context.Add(movie);
            await context.SaveChangesAsync();
            return NoContent();
        }

        private void annotateActorsOrder(Movie movie)
        {
            if(movie.movieActors != null)
            {
                for(int i = 0; i < movie.movieActors.Count; i++)
                {
                    movie.movieActors[i].order = i;
                }
            }
        }
    }
}
