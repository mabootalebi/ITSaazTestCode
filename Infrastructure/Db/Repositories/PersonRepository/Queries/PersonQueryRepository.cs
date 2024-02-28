using Contracts.DTOs.Person;
using Domain.Entities;
using Domain.RepositoryInterfaces.IPersonRepository.Queries;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Db.Repositories.PersonRepository.Queries
{
    public class PersonQueryRepository: IPersonQueryRepository
    {
        private readonly RepositoryDbContext _dbContext;
        public PersonQueryRepository(RepositoryDbContext dbContext) => _dbContext = dbContext;
        

        public async Task<IEnumerable<Person>?> GetAllAsync()
        {
            return await _dbContext.People.ToListAsync();
        }

        public async Task<Person?> GetByIdAsync(int id)
        {
            return await _dbContext.People
                                   .Where(t => t.Id == id)
                                   .FirstOrDefaultAsync();
        }

        public IQueryable<Person> FetchFilteredQuery(FilterDto dto)
        {
            var query = _dbContext.People.AsQueryable();

            if (dto.FirstName is not null)
                query = query.Where(t => t.Firstname.Equals(dto.FirstName)).AsQueryable();

            if (dto.LastName is not null)
                query = query.Where(t => t.Lastname.Equals(dto.LastName)).AsQueryable();

            if (dto.DateOfBirth is not null)
                query = query.Where(t => t.DateOfBirth.Equals(dto.DateOfBirth)).AsQueryable();

            if (dto.Email is not null)
                query = query.Where(t => t.Email.Equals(dto.Email)).AsQueryable();

            return query;
        }
    }
}
