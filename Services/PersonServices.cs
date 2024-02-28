using Contracts.DTOs;
using Contracts.DTOs.Person;
using Domain.Entities;
using Domain.RepositoryInterfaces.IPersonRepository.Commands;
using Domain.RepositoryInterfaces.IPersonRepository.Queries;
using Services.Interfaces;

namespace Services
{
    public class PersonServices: IPersonServices
    {
        private readonly IPersonQueryRepository _queryRepository;
        private readonly IPersonCommandRepository _commandRepository;

        public PersonServices(IPersonQueryRepository queryRepository, IPersonCommandRepository commandRepository)
        {
            _queryRepository = queryRepository;
            _commandRepository = commandRepository;
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

        public async Task<ResultDto<FetchPersonDto>> CreateAsync(CreatePersonDto dto)
        {
            var result = ValidatingCreateRequest(dto);
            if (result.HasError)
                return result;

            var createdPerson = await _commandRepository.CreateAsync(CreatePersonInstance(dto));
            result.Result = CreatePersonDtoInstance(createdPerson);
            return result;
        }

        private ResultDto<FetchPersonDto> ValidatingCreateRequest(CreatePersonDto dto)
        {
            var personalInfoFilter = new FilterDto
            {
                DateOfBirth = dto.DateOfBirth,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            var customerWithTheSamePersonalInfoAlreadyExists = _queryRepository.FetchFilteredQuery(personalInfoFilter).Any();
            if (customerWithTheSamePersonalInfoAlreadyExists)
                return new ResultDto<FetchPersonDto>
                {
                    HasError = true,
                    Message = "Customer with the same firstname, lastname and dateOfbirth already exists."
                };


            var theSameEmailAlreadyExists = _queryRepository.FetchFilteredQuery(new FilterDto { Email = dto.Email }).Any();
            if (theSameEmailAlreadyExists)
                return new ResultDto<FetchPersonDto>
                {
                    HasError = true,
                    Message = "This email address is already taken"
                };

            return new ResultDto<FetchPersonDto>();
        }


        private Person CreatePersonInstance(CreatePersonDto dto)
        {
            return new Person
            {
                DateOfBirth = dto.DateOfBirth,
                Email = dto.Email,
                Firstname = dto.FirstName,
                Lastname = dto.LastName,
                PhoneNumber = dto.NormalizedPhonNumber
            };
        }
        private FetchPersonDto CreatePersonDtoInstance(Person person)
        {
            return new FetchPersonDto
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
