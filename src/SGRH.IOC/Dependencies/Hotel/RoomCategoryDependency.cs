using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SGRH.Application.Dtos.Hotel.RoomCategory;
using SGRH.Application.Dtos.Hotel.RoomCategory.Validators;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Application.Services.Hotel;
using SGRH.Application.UseCases.Hotel.RoomCategory;
using SGRH.Persistence.Repositories.Hotel;

namespace SGRH.IOC.Dependencies.Hotel
{
    public static class RoomCategoryDependency
    {
        public static void AddRoomCategoryDependency(this IServiceCollection service)
        {
            // Repositorio y Servicio
            service.AddScoped<IRoomCategoryRepository, RoomCategoryRepository>();
            service.AddTransient<IRoomCategoryService, RoomCategoryService>();

            // Validadores de FluentValidation
            service.AddScoped<IValidator<CreateRoomCategoryDto>, CreateRoomCategoryValidator>();
            service.AddScoped<IValidator<ModifyRoomCategoryDto>, ModifyRoomCategoryValidator>();
            service.AddScoped<IValidator<DisableRoomCategoryDto>, DisableRoomCategoryValidator>();

            // Casos de Uso
            service.AddTransient<RoomCategoryNameMustBeUnique>();
            service.AddTransient<RoomCategoryMustNotHaveAssociatedRooms>();
        }
    }
}