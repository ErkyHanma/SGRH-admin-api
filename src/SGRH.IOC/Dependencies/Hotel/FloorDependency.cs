using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SGRH.Application.Dtos.Hotel.Floor;
using SGRH.Application.Dtos.Hotel.Floor.Validators;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Application.Services.Hotel;
using SGRH.Persistence.Repositories.Hotel;

namespace SGRH.IOC.Dependencies.Hotel
{
    public static class FloorDependency
    {
        public static void AddFloorDependency(this IServiceCollection service)
        {
            // Repositorio y Servicio de Floor
            service.AddScoped<IFloorRepository, FloorRepository>();
            service.AddTransient<IFloorService, FloorService>();

            // Validadores de FluentValidation para Floor
            service.AddScoped<IValidator<CreateFloorDto>, CreateFloorValidator>();
            service.AddScoped<IValidator<ModifyFloorDto>, ModifyFloorValidator>();
            service.AddScoped<IValidator<DisableFloorDto>, DisableFloorValidator>();
        }
    }
}