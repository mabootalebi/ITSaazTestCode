using Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<RepositoryDbContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {                        
                        throw;
                    }
                }
            }
            return webApp;
        }
    }
}
