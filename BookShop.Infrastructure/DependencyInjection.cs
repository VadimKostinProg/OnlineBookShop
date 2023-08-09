using BookShop.Core.Domain.RepositoryContracts;
using BookShop.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IRepository, RepositoryBase>();

            return services;
        }
    }
}
