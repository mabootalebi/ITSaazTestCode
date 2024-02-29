using Contracts.DTOs.Person;
using Contracts.Enums;
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
            return person is not null ? Ok(person) : NotFound($"There is no person with Id: {id}");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePersonDto dto)
        {
            var result = await _personServices.CreateAsync(dto);
            return result.Status == StatusEnum.Success? 
                Created(new Uri(Request.GetEncodedUrl() + "/" + result.Result?.Id), result.Result): BadRequest(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePersonDto dto)
        {
            dto.Id = id;
            var result = await _personServices.UpdateAsync(dto);
            return result.Status switch
            {
                StatusEnum.Success => Ok(result.Result),
                StatusEnum.NotFound => NotFound(result.Message),
                StatusEnum.BadRequest => BadRequest(result.Message),
                _ => throw new InvalidOperationException()
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _personServices.DeleteAsync(id);
            return result.Status switch
            {
                StatusEnum.Success => NoContent(),
                StatusEnum.NotFound => NotFound(result.Message),
                _ => throw new InvalidOperationException()
            };
        }
    }
}
