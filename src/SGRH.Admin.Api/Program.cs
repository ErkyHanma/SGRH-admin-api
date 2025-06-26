using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using SGRH.Application.Common.Logging;
using SGRH.Application.Common.Mappers.ServiceModule;
using SGRH.Application.Interfaces.Mappers.ServiceModule;
using SGRH.Application.Interfaces.Repositories.ReservationModule;
using SGRH.Application.Interfaces.Repositories.ServiceModule;
using SGRH.Application.Interfaces.Services.ReservationModule;
using SGRH.Application.Interfaces.Services.Service_Module;
using SGRH.Application.Services.ReservationModule;
using SGRH.Application.Services.ServiceModule;
using SGRH.Persistence.Context;
using SGRH.Persistence.Repositories.ReservationModule;
using SGRH.Persistence.Repositories.Service_Module;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

builder.Configuration["ConnectionStrings:SGRHConnection"] = connectionString;


builder.Services.AddDbContext<SGRHContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("SGRHConnection")));
builder.Services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));

// Add services to the container.
//ReservationModule
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddTransient<IReservationService, ReservationService>();

//ServiceModule
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddTransient<IServiceService, ServiceService>();
builder.Services.AddScoped<IServiceMapper, ServiceMapper>();



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
