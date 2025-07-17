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
using SGRH.Application.UseCases.Hotel.Room;
using SGRH.Application.Interfaces.UseCases;

namespace SGRH.IOC.Dependencies.Hotel
{
    public static class RoomDependency
    {
        public static void AddRoomDependency(this IServiceCollection service)
        {
            service.AddScoped<IRoomRepository, RoomRepository>();
            service.AddTransient<IRoomService, RoomService>();

            //Use cases

            service.AddScoped<IMustNotBeOccupied<RoomDto>, RoomMustNotBeOccupied>();
            service.AddScoped<IMustExistValidator<int>, RoomCategoryMustExist>(); // <- Este mas tarde puede ser movido a una clase para dependencias compartidas (Lo usan Room y Rate)
            service.AddScoped<IMustExistValidator<int>, RoomFloorMustExist>();


            //Fluent validation

            service.AddScoped<IValidator<CreateRoomDto>, CreateRoomValidator>();
            service.AddScoped<IValidator<ModifyRoomDto>, ModifyRoomValidator>();
            service.AddScoped<IValidator<DisableRoomDto>, DisableRoomValidator>();
           
        }
    }
}
