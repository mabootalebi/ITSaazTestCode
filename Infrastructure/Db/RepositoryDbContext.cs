using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Db
{
    public sealed class RepositoryDbContext: DbContext
    {
        public RepositoryDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Person> People { get; set; }

    }
}
