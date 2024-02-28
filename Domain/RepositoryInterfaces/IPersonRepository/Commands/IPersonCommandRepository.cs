using Domain.Entities;


namespace Domain.RepositoryInterfaces.IPersonRepository.Commands
{
    public interface IPersonCommandRepository
    {
        Task<Person> CreateAsync(Person person);
    }
}
