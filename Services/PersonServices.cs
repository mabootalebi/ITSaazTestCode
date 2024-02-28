using Contracts.DTOs;
using Contracts.DTOs.Person;
using Contracts.Enums;
using Domain.Entities;
using Domain.RepositoryInterfaces.IPersonRepository.Commands;
using Domain.RepositoryInterfaces.IPersonRepository.Queries;
using Services.Interfaces;

namespace Services
{
    public class PersonServices : IPersonServices
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
            return person is null ? null : new FetchPersonDto
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
            var result = ValidatingCreateOrUpdateRequest(dto);
            if (result.Status != StatusEnum.Success)
                return result;

            var person = CreatePersonInstance(dto);
            await _commandRepository.CreateAsync(person);
            result.Result = CreatePersonDtoInstance(person);
            return result;
        }

        private ResultDto<FetchPersonDto> ValidatingCreateOrUpdateRequest(CreatePersonDto dto, int? id = null)
        {
            if (AnotherCustomerWithTheSamePersonalInfoAlreadyExists(dto, id))
                return new ResultDto<FetchPersonDto>
                {
                    Status = StatusEnum.BadRequest,
                    Message = "Customer with the same firstname, lastname and dateOfbirth already exists."
                };

            if (TheSameEmailAlreadyExists(dto.Email, id))
                return new ResultDto<FetchPersonDto>
                {
                    Status = StatusEnum.BadRequest,
                    Message = "This email address is already taken"
                };

            return new ResultDto<FetchPersonDto>();
        }

        private bool AnotherCustomerWithTheSamePersonalInfoAlreadyExists(CreatePersonDto dto, int? id = null)
        {
            var personalInfoFilter = new FilterDto
            {
                DateOfBirth = dto.DateOfBirth,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };
            var result = _queryRepository.FetchFilteredQuery(personalInfoFilter);

            if (id.HasValue)
                result = result.Where(t => t.Id != id.Value);

            return result.Any();
        }

        private bool TheSameEmailAlreadyExists(string email, int? id = null)
        {
            var result = _queryRepository.FetchFilteredQuery(new FilterDto { Email = email });
            if (id.HasValue)
                result = result.Where(t => t.Id != id.Value);

            return result.Any();
        }

        public async Task<ResultDto<FetchPersonDto>> UpdateAsync(UpdatePersonDto dto)
        {
            var fetchPersonResult = await TryFetchPersonById(dto.Id);
            if (fetchPersonResult.Status != StatusEnum.Success)
                return new ResultDto<FetchPersonDto>()
                {
                    Status = fetchPersonResult.Status,
                    Message = fetchPersonResult.Message
                };

            var result = ValidatingCreateOrUpdateRequest(dto, dto.Id);
            if (result.Status != StatusEnum.Success)
                return result;

            var person = fetchPersonResult.Result;
            UpdatePersonEntityProperties(person!, dto);

            await _commandRepository.UpdateAsync(person!);
            result.Result = CreatePersonDtoInstance(person!);
            return result;
        }

        private async Task<ResultDto<Person>> TryFetchPersonById(int id)
        {
            var person = await _queryRepository.GetByIdAsync(id);
            if (person is null)
                return new ResultDto<Person>
                {
                    Status = StatusEnum.NotFound,
                    Message = $"Not found person with Id: {id}"
                };

            return new ResultDto<Person>
            {
                Result = person
            };
        }
        private void UpdatePersonEntityProperties(Person person, UpdatePersonDto dto)
        {
            person.Email = dto.Email;
            person.Firstname = dto.FirstName;
            person.Lastname = dto.LastName;
            person.PhoneNumber = dto.PhoneNumber;
            person.DateOfBirth = dto.DateOfBirth;
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
