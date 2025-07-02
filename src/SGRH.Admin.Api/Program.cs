using DotNetEnv;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.Hotel.Rate;
using SGRH.Application.Dtos.Hotel.Rate.Validators;
using SGRH.Application.Dtos.Hotel.Room;
using SGRH.Application.Dtos.Hotel.Room.Validators;
using SGRH.Application.Interfaces.Mappers.Hotel;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Application.Services.Hotel;
using SGRH.Persistence.Repositories.Hotel;
using SGRH.Application.Interfaces.Repositories.Report;
using SGRH.Application.Interfaces.Services.Report;
using SGRH.Application.Services.Report;
using SGRH.Persistence.Repositories.Report;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Application.Interfaces.Services.ReservationModule;
using SGRH.Application.Services.ReservationModule;
using SGRH.Persistence.Repositories.ReservationModule;
using SGRH.Application.Common.Mappers.ServiceModule;
using SGRH.Application.Interfaces.Mappers.ServiceModule;
using SGRH.Application.Interfaces.Repositories.ServiceModule;
using SGRH.Application.Interfaces.Services.Service_Module;
using SGRH.Application.Services.ServiceModule;
using SGRH.Persistence.Repositories.Service_Module;

using SGRH.Persistence.Context;

namespace SGRH.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load environment variables
            Env.Load();

            var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
            builder.Configuration["ConnectionStrings:SGRHConnection"] = connectionString;

            // Add services to the container

            // Logger
            builder.Services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));

            // FluentValidation - Hotel module
            builder.Services.AddScoped<IValidator<CreateRoomDto>, CreateRoomValidator>();
            builder.Services.AddScoped<IValidator<ModifyRoomDto>, ModifyRoomValidator>();
            builder.Services.AddScoped<IValidator<DisableRoomDto>, DisableRoomValidator>();

            builder.Services.AddScoped<IValidator<CreateRateDto>, CreateRateValidator>();
            builder.Services.AddScoped<IValidator<UpdateRateDto>, UpdateRateValidator>();
            builder.Services.AddScoped<IValidator<DeleteRateDto>, DeleteRateValidator>();

            // DbContext
            builder.Services.AddDbContext<SGRHContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("SGRHConnection"));
            });

            // Hotel Module
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddTransient<IRoomService, RoomService>();

            builder.Services.AddScoped<IRatesRepository, RatesRepository>();
            builder.Services.AddTransient<IRatesService, RatesService>();
            builder.Services.AddTransient<IRateMapper, RateMapper>();

            // Report Module
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddTransient<IReportService, ReportService>();

            // Modulo Tarifa (!!!!!!!!!!!!!)

            builder.Services.AddScoped<IRatesRepository, RatesRepository>();
            builder.Services.AddTransient<IRatesService, RatesService>();

            // Reservations 
            builder.Services.AddTransient<IRateMapper, RateMapper>();

            builder.Services.AddScoped<IReservationServiceRepository, ReservationServiceRepository>();
            builder.Services.AddTransient<IReservationServiceService, ReservationServiceService>();

            // Service Module
            builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
            builder.Services.AddTransient<IServiceService, ServiceService>();
            builder.Services.AddScoped<IServiceMapper, ServiceMapper>();

            // API setup
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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
