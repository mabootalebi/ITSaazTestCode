using Contracts.DTOs.Person;
using Domain.Entities;


namespace Domain.RepositoryInterfaces.IPersonRepository.Queries
{
    public interface IPersonQueryRepository
    {
        Task<IEnumerable<Person>?> GetAllAsync();
        Task<Person?> GetByIdAsync(int id);
        IQueryable<Person> FetchFilteredQuery(FilterDto dto);
    }
}
