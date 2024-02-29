using Domain.Entities;
using Domain.RepositoryInterfaces.IPersonRepository.Commands;


namespace Infrastructure.Db.Repositories.PersonRepository.Commands
{
    public class PersonCommandRepository: IPersonCommandRepository
    {
        private readonly RepositoryDbContext _dbContext;
        public PersonCommandRepository(RepositoryDbContext dbContext) => _dbContext = dbContext;

        public async Task CreateAsync(Person person)
        {
            _dbContext.People.Add(person);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Person person)
        {
            _dbContext.People.Update(person);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Person person)
        {
            _dbContext.People.Remove(person);
            await _dbContext.SaveChangesAsync();
        }
    }
}
