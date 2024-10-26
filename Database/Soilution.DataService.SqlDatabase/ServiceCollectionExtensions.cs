using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Soilution.DataService.SqlRepository
{
    public static class ServiceCollectionExtensions
    {
        public static void SetupSqlAcessorLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseAccessorSettings>(configuration.GetSection("Database"));
        }
    }
}
