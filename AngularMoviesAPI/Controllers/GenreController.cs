using AngularMoviesAPI.Entities;
using AngularMoviesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.Controllers
{
    #region // parameters explained
    /*
     * Query Strings
     * name - value pairs in URLs
     * for example: api/genres?id=5&surname=Gabi
     */
    /*
     * Form values
     * [HttpPost]
     * public void post([FormBody] genre value) {}
     */
    #endregion
    #region Model Binder Configurations
    /*
     * Model binder configurations
     * BindRequired: indicated something parameter is required
     * for example:
     * [HttpGet("{Id:int}/{param2=felipe}")]
     * public ActionResult<Genre> Get(int id, [BindRequired]string param2) {}
     * BindNever
     * FromHeader
     * FromQuery
     * FromRoute
     * FromaForm
     * FromServices
     * FromBody
     */
    #endregion
    #region Model validation / Validating Models
    /*
     * Model valdaiton / Validating Models
     * Attribute Validations
     * Required
     * StringLength
     * Range
     * CreditCard
     * Compare
     * Phone format
     * Regular Expression
     * Url
     * BindRequired
     */
    #endregion
    [Route("api/genres")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IRepository repository;
        public GenreController(IRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet] // api/genres
        [HttpGet("list")] // api/genres/list
        [HttpGet("/allGenres")] //allGenres : will override the previous route attribute settings
        // response to http request on webapi
        // !!! because the return type changed to task<genre list>, hence here need to be updated to async taks as well
        public async Task<ActionResult<List<Genre>>> Get()
        {
            // Here is getting from in-memory database, will be changed to webapi request
            return await repository.getAllGenres();
        }
        [HttpGet("{Id:int}/{param2=felipe}")] //api/genres/{Id} : we can add int to restrict the input parameter in the url to avoid user access this endpoint by mistake
        // If here using the same method signature Get will caused exception at running time
        public ActionResult<Genre> Get(int id, string param2)
        {
            var genre = repository.getGenreById(id);
            if(genre == null)
            {
                return NotFound();
            }
            return genre;
        }
        [HttpGet("{Id:int}")]
        public IActionResult Get(int id)
        {
            var genre = repository.getGenreById(id);
            return Ok(genre); // ok means 200 okay and genre wil be return as a response
        }
        [HttpPost]
        public ActionResult post([FromBody] Genre genre)
        {
            /*
             * Adding [ApiController] to avoid writing the below codes
             */
            //// ModelState is invalid meaning the request is incomplete
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            return NoContent();
        }

        [HttpPut]
        public ActionResult put([FromBody] Genre genre) 
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            // added [ApiController]
            return NoContent();
        }
        [HttpDelete]
        public ActionResult delete()
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            // added [ApiController] hence no need above code 
            return NoContent();
        }
    }
}
