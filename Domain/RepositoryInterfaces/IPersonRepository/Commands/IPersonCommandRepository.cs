using Domain.Entities;


namespace Domain.RepositoryInterfaces.IPersonRepository.Commands
{
    public interface IPersonCommandRepository
    {
        Task CreateAsync(Person person);
        Task UpdateAsync(Person person);
    }
}
