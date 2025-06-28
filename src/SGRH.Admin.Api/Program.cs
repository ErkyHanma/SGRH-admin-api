using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.Hotel.Rate;
using SGRH.Application.Dtos.Hotel.Rate.Validators;
using SGRH.Application.Dtos.Hotel.Room;
using SGRH.Application.Dtos.Hotel.Room.Validators;
using SGRH.Application.Interfaces.Mappers.Hotel;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Repositories.Report;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Application.Interfaces.Services.Report;
using SGRH.Application.Services.Hotel;
using SGRH.Application.Services.Report;
using SGRH.Persistence.Context;
using SGRH.Persistence.Repositories.Hotel;
using SGRH.Persistence.Repositories.Report;

namespace SGRH.Api
{
    //references
    public class Program
    {
        //references
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Logger

            builder.Services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));

            // Fluent Validation

            builder.Services.AddScoped<IValidator<CreateRoomDto>, CreateRoomValidator>();
            builder.Services.AddScoped<IValidator<ModifyRoomDto>, ModifyRoomValidator>();
            builder.Services.AddScoped<IValidator<DisableRoomDto>, DisableRoomValidator>();

            builder.Services.AddScoped<IValidator<CreateRateDto>, CreateRateValidator>();
            builder.Services.AddScoped<IValidator<UpdateRateDto>, UpdateRateValidator>(); 
            builder.Services.AddScoped<IValidator<DeleteRateDto>, DeleteRateValidator >();


            // Db Context

            builder.Services.AddDbContext<SGRHContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("SGRH"));
            });

            // Modulo de habitaciones

            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddTransient<IRoomService, RoomService>();

            // Modulo de Reportes

            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddTransient<IReportService, ReportService>();

            // Modulo Tarifa (!!!!!!!!!!!!!)

            builder.Services.AddScoped<IRatesRepository, RatesRepository>();
            builder.Services.AddTransient<IRatesService, RatesService>();
            builder.Services.AddTransient<IRateMapper, RateMapper>();


            // ...

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}