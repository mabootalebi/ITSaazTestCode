using Contracts.DTOs.Person;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonServices _personServices;

        public PersonController(ILogger<PersonController> logger, IPersonServices personServices)
        {
            _logger = logger;
            _personServices = personServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var people = await _personServices.GetAllPeopleAsync();
            return Ok(people);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var person = await _personServices.GetPersonByIdAsync(id);
            return person is not null? Ok(person): NotFound($"There is no person with Id: {id}");
        }

        [HttpPost()]
        public async Task<IActionResult> Create(CreatePersonDto dto)
        {
            var result = await _personServices.CreateAsync(dto);
            return result.HasError? BadRequest(result.Message) : Created(new Uri(Request.GetEncodedUrl() + "/" + result.Result?.Id), result.Result);
        }
    }
}
