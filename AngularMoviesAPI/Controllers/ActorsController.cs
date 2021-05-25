using AngularMoviesAPI.DTOs;
using AngularMoviesAPI.Entities;
using AngularMoviesAPI.helpers;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.Controllers
{
    [Route("movie/actors")]
    // apicontroller will help us to use different actionresult, like NoContent()
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorageService storageService;
        

        private readonly string containerName = "actors";
        public ActorsController(ApplicationDbContext context, IMapper mapper, IFileStorageService storageService)
        {
            this.context = context;
            this.mapper = mapper;
            this.storageService = storageService;
        }

        // api/actors
        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            // Calculate how many actors
            var queryable = context.Actors.AsQueryable();
            await HttpContext.InsertParametersPaginationInHeader(queryable);
            // Because pagination 
            var actors = await queryable.OrderBy(x => x.name).Paginate(paginationDTO).ToListAsync(); 
            //var actors = await context.Actors.ToListAsync();
            return mapper.Map<List<ActorDTO>>(actors);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var actor = await context.Actors.FirstOrDefaultAsync(x => x.id == id);
            if(actor != null)
            {
                return mapper.Map<ActorDTO>(actor);
            }
            return NoContent();

        }
        [HttpPost("SearchByName")]
        public async Task<ActionResult<List<ActorsMovieDTO>>> SearchByName([FromBody] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new List<ActorsMovieDTO>();
            }
            return await context.Actors
                .Where(x => x.name.Contains(name))
                .OrderBy(x => x.name)
                .Select(x => new ActorsMovieDTO
                {
                    id = x.id,
                    name = x.name,
                    picture = x.picture
                })
                .Take(5)
                .ToListAsync();
        }
        // api/actors
        // Form
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreationDTO actorCreationDTO)
        {
            var actor = mapper.Map<Actor>(actorCreationDTO);
            if (actorCreationDTO.picture != null) 
            {
                // saveFile(container name : string, file : IFormFile), the return value is the Azure storage file saving path
                actor.picture = await storageService.saveFile(containerName, actorCreationDTO.picture);
            }
            context.Add(actor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        // api/actor/id=?
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreationDTO actorCreationDTO)
        {
            var actor = await context.Actors.FirstOrDefaultAsync(x => x.id == id);
            if(actor == null)
            {
                return NotFound();
            }
            actor = mapper.Map(actorCreationDTO, actor);
            if(actorCreationDTO.picture != null)
            {
                actor.picture = await storageService.editFile(containerName, actorCreationDTO.picture, actor.picture);
            }
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var actor = await context.Actors.FirstOrDefaultAsync(x => x.id == id);
            if (actor == null)
            {
                return NotFound();
            }
            context.Remove(actor);
            await context.SaveChangesAsync();

            await storageService.deleteFile(actor.picture, containerName);
            return Ok();
        }
    }
}
