using Core.SGRH.Application.Interfaces.Repositories.UserManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGRH.Persistence.Repositories.UserManagement;

namespace SGRH.Persistence.Repositories.UserManagement
{
    public static class UserManagementServiceRegistration
    {
        public static IServiceCollection AddUserManagementRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SGRH");

            services.AddScoped<IClientRepository>(provider =>
                new ClientRepository(connectionString));

            return services;
        }
    }
}
