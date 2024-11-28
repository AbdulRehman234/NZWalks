using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace NZWalks.API.Data.DataBaseConfig
{
    public static class DbConfigration
    {
        public static IServiceCollection AddDbConnetionStringThroughExtention(this IServiceCollection service,IConfiguration configuration )
        {
            service.AddSqlServer<NZWalksDbContext>(configuration.GetConnectionString("DefaultConnection"));
            return service;
        }

        public static IServiceCollection AddAuthConnectionStringThroughExtention(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddSqlServer<NZWalksAuthDbContext>(configuration.GetConnectionString("NZWalksAuthConnectionString"));
            return services;
        }
    }
}
