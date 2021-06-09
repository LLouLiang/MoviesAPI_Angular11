using AngularMoviesAPI.DTOs;
using AngularMoviesAPI.Entities;
using AngularMoviesAPI.Filters;
using AngularMoviesAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.Controllers
{
    [Route("movie/genres")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        // Logger service injection
        private readonly ILogger<GenresController> logger;
        // local in memory database repository injection
        private readonly IRepository repository;
        // database context 
        private readonly ApplicationDbContext context;
        // Add AutoMapper dependency
        private readonly IMapper mapper;
        public GenresController(ILogger<GenresController> logger, 
                                IRepository repository, 
                                ApplicationDbContext context,
                                IMapper mapper)
        {
            this.logger = logger;
            this.repository = repository;
            this.context = context;
            this.mapper = mapper;
        }

        // Get
        [HttpGet]
        [ServiceFilter(typeof(CustomActionFilter))]
        public async Task<List<GenreDTO>> get()
        {
            //return await repository.getAllGenres(); // getting genres list from in memory list
            var genres = await context.Genres.OrderBy(x=>x.Name).ToListAsync();
            /*
             * Add automapper to avoid below model mapping
             */
            //var genresDTO = new List<GenreDTO>();
            //foreach(var genre in genres)
            //{
            //    genresDTO.Add(new GenreDTO {Id = genre.Id, Name = genre.Name});
            //}
            //return genresDTO;
            return mapper.Map<List<GenreDTO>>(genres);
        }

        [HttpGet("{name}")]
        [HttpGet("getfiction")]
        public List<Genre> get(string name)
        {
            // for testing purpose
            return new List<Genre>() { new Genre() { Id = 1, Name = "fiction" } };
        }

        [HttpGet("{Id:int}")] // movie/genres/Id=?
        public async Task<ActionResult<GenreDTO>> get(int Id)
        {
            var genres = await context.Genres.FirstOrDefaultAsync(x => x.Id == Id);
            if(genres == null)
            {
                return NotFound();
            }
            return mapper.Map<GenreDTO>(genres);
            
        }

        //[HttpPost] change to use GenreDTO 
        //public async Task<ActionResult> post([FromBody] Genre genre)
        //{
        //    context.Add(genre);
        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}
        [HttpPost]
        public async Task<ActionResult> post([FromBody] GenreCreationDTO genreCreationDTO)
        {
            var genre = mapper.Map<Genre>(genreCreationDTO);
            context.Add(genre);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> put(int id, [FromBody] GenreCreationDTO genrecreationDTO)
        {
            var genre = mapper.Map<Genre>(genrecreationDTO);
            genre.Id = id;
            context.Entry(genre).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> delete(int id)
        {
            
            var genre = await context.Genres.FirstOrDefaultAsync(x => x.Id == id);
            logger.LogInformation("delete genre has been called, {0} Genre Id has been removed", id);
            if (genre != null)
            {
                context.Remove(genre);
                await context.SaveChangesAsync();
            }
            return NoContent();
        }
    }
}
