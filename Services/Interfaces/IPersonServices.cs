using Contracts.DTOs.Person;

namespace Services.Interfaces
{
    public interface IPersonServices
    {
        Task<IEnumerable<FetchPersonDto>?> GetAllPeopleAsync();
        Task<FetchPersonDto?> GetPersonByIdAsync(int id);
    }
}
