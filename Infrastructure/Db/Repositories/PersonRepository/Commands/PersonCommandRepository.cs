using Domain.Entities;
using Domain.RepositoryInterfaces.IPersonRepository.Commands;


namespace Infrastructure.Db.Repositories.PersonRepository.Commands
{
    public class PersonCommandRepository: IPersonCommandRepository
    {
        private readonly RepositoryDbContext _dbContext;
        public PersonCommandRepository(RepositoryDbContext dbContext) => _dbContext = dbContext;

        public async Task<Person> CreateAsync(Person person)
        {
            _dbContext.People.Add(person);
            await _dbContext.SaveChangesAsync();
            return person;
        }
    }
}
