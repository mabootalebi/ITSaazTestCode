using Domain.RepositoryInterfaces.IPersonRepository.Queries;
using Infrastructure.Db.Repositories.PersonRepository.Queries;
using Services;
using Services.Interfaces;

namespace API
{
    public static class IoC
    {
        public static void ResolveIoC(this WebApplicationBuilder builder)
        {
            #region repositories
            builder.Services.AddTransient<IPersonQueryRepository, PersonQueryRepository>();
            #endregion

            #region Services
            builder.Services.AddTransient<IPersonServices, PersonServices>();
            #endregion
        }
    }
}
