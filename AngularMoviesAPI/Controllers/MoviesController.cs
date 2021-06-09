using AngularMoviesAPI.DTOs;
using AngularMoviesAPI.Entities;
using AngularMoviesAPI.helpers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IFileStorageService storageService;
        private string container = "movies";

        public MoviesController(IMapper mapper, ApplicationDbContext context, IFileStorageService storageService)
        {
            this.mapper = mapper;
            this.context = context;
            this.storageService = storageService;
        }
        [HttpGet("id:int")]
        public async Task<ActionResult<MovieDTO>> Get(int id)
        {
            var movie = await context.Movie
                .Include(x => x.movieGenres).ThenInclude(x => x.Genre)
                .Include(x => x.movieTheaterMovies).ThenInclude(x => x.movieTheater)
                .Include(x => x.movieActors).ThenInclude(x => x.actor)
                .FirstOrDefaultAsync(x => x.id == id);
            if(movie == null)
            {
                return NotFound();
            }
            var dto = mapper.Map<MovieDTO>(movie);
            dto.actors = dto.actors.OrderBy(x => x.order).ToList();
            return dto;
        }
        [HttpGet("PostGet")]
        public async Task<ActionResult<MoviePostGetDTO>> PostGet()
       {
            var movietheaters = await context.MovieTheater.OrderBy(x=>x.name).ToListAsync();
            var genres = await context.Genres.OrderBy(x=>x.Name).ToListAsync();
            var movieTheaterDTO = mapper.Map<List<MovieTheaterDTO>>(movietheaters);
            var genreDTO = mapper.Map<List<GenreDTO>>(genres);

            return new MoviePostGetDTO() { genres = genreDTO, movieTheaters = movieTheaterDTO };
        }
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
