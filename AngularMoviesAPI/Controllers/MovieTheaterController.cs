using AngularMoviesAPI.DTOs;
using AngularMoviesAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.Controllers
{
    [Route("movie/movietheater")]
    [ApiController]
    public class MovieTheaterController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public MovieTheaterController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieTheaterDTO>>> Get()
        {
            var theaters = await context.MovieTheater.OrderBy(x=>x.name).ToListAsync();
            return mapper.Map<List<MovieTheaterDTO>>(theaters);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieTheaterDTO>> Get(int id)
        {
            var theater = await context.MovieTheater.FirstOrDefaultAsync(x => x.id == id);
            if (theater == null) return NoContent();
            return mapper.Map<MovieTheaterDTO>(theater);
        }

        [HttpPost]
        public async Task<ActionResult> Post(MovieTheaterCreationDTO movievTheaterCreationDTO)
        {
            var new_theater = mapper.Map<MovieTheater>(movievTheaterCreationDTO);
            context.Add(new_theater);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, MovieTheaterCreationDTO movietheater)
        {
            var theater = await context.MovieTheater.FirstOrDefaultAsync(x => x.id == id);
            if (theater == null) return NoContent();
            theater = mapper.Map(movietheater, theater);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var theater = await context.MovieTheater.FirstOrDefaultAsync(x=>x.id == id);
            if (theater == null) return NoContent();
            context.Remove(theater);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
