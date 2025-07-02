using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SGRH.Application.Dtos.Hotel.Rate.Validators;
using SGRH.Application.Dtos.Hotel.Rate;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Application.Services.Hotel;
using SGRH.Persistence.Repositories.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGRH.Application.Dtos.Hotel.Room.Validators;
using SGRH.Application.Dtos.Hotel.Room;

namespace SGRH.IOC.Dependencies.Hotel
{
    public static class RoomDependency
    {
        public static void AddRoomDependency(this IServiceCollection service)
        {
            service.AddScoped<IRoomRepository, RoomRepository>();
            service.AddTransient<IRoomService, RoomService>();

            //Fluent validation

            service.AddScoped<IValidator<CreateRoomDto>, CreateRoomValidator>();
            service.AddScoped<IValidator<ModifyRoomDto>, ModifyRoomValidator>();
            service.AddScoped<IValidator<DisableRoomDto>, DisableRoomValidator>();
        }
    }
}
