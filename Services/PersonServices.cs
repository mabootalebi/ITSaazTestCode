using Contracts.DTOs.Person;
using Domain.RepositoryInterfaces.IPersonRepository.Queries;
using Services.Interfaces;


namespace Services
{
    public class PersonServices: IPersonServices
    {
        private readonly IPersonQueryRepository _queryRepository;
        public PersonServices(IPersonQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public async Task<IEnumerable<FetchPersonDto>?> GetAllPeopleAsync()
        {
            return (await _queryRepository.GetAllAsync())?
                    .Select(t => new FetchPersonDto 
                    {
                        DateOfBirth = t.DateOfBirth,
                        Email = t.Email,
                        FirstName = t.Firstname, 
                        LastName = t.Lastname,
                        PhoneNumber = t.PhoneNumber,
                        Id = t.Id
                    }).ToList();
        }

        public async Task<FetchPersonDto?> GetPersonByIdAsync(int id)
        {
            var person = await _queryRepository.GetByIdAsync(id);
            return person is null? null : new FetchPersonDto
                {
                    DateOfBirth = person.DateOfBirth,
                    Email = person.Email,
                    FirstName = person.Firstname,
                    LastName = person.Lastname,
                    PhoneNumber = person.PhoneNumber,
                    Id = person.Id
                };
        }
    }
}
