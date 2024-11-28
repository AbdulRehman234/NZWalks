using NZWalks.API.Interfaces.Walks;
using NZWalks.API.Repositories.Walks;

namespace NZWalks.API.Configrations
{
    public static class ServicesConfigrations
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IWalksRepository,WalkRepository>();

            return services;
        }
    }
}
