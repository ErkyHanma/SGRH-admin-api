using Core.Application.Interfaces.Repositories.UserManagement;
using Microsoft.Extensions.DependencyInjection;

namespace SGRH.Persistence.Repositories.UserManagement
{
    public static class UserManagementServiceRegistration
    {
        public static IServiceCollection AddUserManagementRepositories(this IServiceCollection services)
        {
            services.AddScoped<IClientRepository, ClientRepository>();
            return services;
        }
    }
}
