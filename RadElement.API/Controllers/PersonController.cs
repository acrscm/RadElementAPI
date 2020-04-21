using System.Threading.Tasks;
using RadElement.Core.DTO;
using RadElement.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RadElement.API.Controllers
{
    /// <summary>
    /// Endpoint for elements set controller
    /// </summary>
    /// <seealso cref="RadElement.API.Controllers.BaseController" />
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("radelement/api/v1")]
    [Authorize(Policy = "UserIdExists")]
    [ApiController]
    public class PersonController : BaseController
    {
        /// <summary>
        /// The person service
        /// </summary>
        private readonly IPersonService personService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonController" /> class.
        /// </summary>
        /// <param name="personService">The person service.</param>
        /// <param name="logger">The logger.</param>
        public PersonController(IPersonService personService, ILogger<PersonController> logger)
        {
            this.personService = personService;
            LoggerInstance = logger;
        }

        /// <summary>
        /// Gets the persons.
        /// </summary>
        /// <returns></returns>
        [HttpGet("persons")]
        public async Task<IActionResult> GetPersons()
        {
            var result = await personService.GetPersons();
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the person.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <returns></returns>
        [HttpGet("persons/{personId}")]
        public async Task<IActionResult> GetPerson(int personId)
        {
            var result = await personService.GetPerson(personId);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Searches the persons.
        /// </summary>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <returns></returns>
        [HttpGet("persons/search")]
        public async Task<IActionResult> SearchPerson([FromQuery]SearchKeyword searchKeyword)
        {
            var result = await personService.SearchPerson(searchKeyword);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates the person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        [HttpPost("persons")]
        public async Task<IActionResult> CreatePerson([FromBody]CreateUpdatePerson person)
        {
            var result = await personService.CreatePerson(person);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the person.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        [HttpPut("persons/{personId}")]
        public async Task<IActionResult> UpdatePerson(int personId, [FromBody]CreateUpdatePerson person)
        {
            var result = await personService.UpdatePerson(personId, person);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Deletes the person.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <returns></returns>
        [HttpDelete("persons/{personId}")]
        public async Task<IActionResult> DeletePerson(int personId)
        {
            var result = await personService.DeletePerson(personId);
            return StatusCode((int)result.Code, result.Value);
        }
    }
}