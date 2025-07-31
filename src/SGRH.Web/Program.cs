using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SGRH.Application.Common.Logging;
using SGRH.Application.Dtos.Hotel.Floor.Validators;
using SGRH.Application.Dtos.Hotel.Floor;
using SGRH.Application.Dtos.Hotel.RoomCategory.Validators;
using SGRH.Application.Dtos.Hotel.RoomCategory;
using SGRH.Application.Interfaces.Mappers.Hotel;
using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Application.Services.Hotel;
using SGRH.Persistence.Context;
using SGRH.Persistence.Repositories.Hotel;
using SGRH.Persistence.Repositories.UserManagement;
using SGRH.Application.Interfaces.Repositories.Report;
using SGRH.Application.Interfaces.Services.Report;
using SGRH.IOC.Dependencies.Hotel;
using SGRH.IOC.Dependencies.Report;
using SGRH.IOC.Dependencies.ReservationModule;
using SGRH.IOC.Dependencies.ServiceModule;
using NuGet.Configuration;
using SGRH.Web.Infrastructure.Http;
using SGRH.Web.Repositories.Interfaces.Hotel;
using SGRH.Web.Repositories;
using SGRH.Web.Infrastructure.Endpoints.Rate;
using SGRH.Web.Infrastructure.Endpoints.Room;


var builder = WebApplication.CreateBuilder(args);

// Load .env variables
//Env.Load();

//var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
//builder.Configuration["ConnectionStrings:SGRHConnection"] = connectionString;

// Logger
builder.Services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));

// FluentValidation - Hotel module

builder.Services.AddScoped<IValidator<CreateFloorDto>, CreateFloorValidator>();
builder.Services.AddScoped<IValidator<ModifyFloorDto>, ModifyFloorValidator>();
builder.Services.AddScoped<IValidator<DisableFloorDto>, DisableFloorValidator>();

builder.Services.AddScoped<IValidator<CreateRoomCategoryDto>, CreateRoomCategoryValidator>();
builder.Services.AddScoped<IValidator<ModifyRoomCategoryDto>, ModifyRoomCategoryValidator>();
builder.Services.AddScoped<IValidator<DisableRoomCategoryDto>, DisableRoomCategoryValidator>();

// DbContext
builder.Services.AddDbContext<SGRHContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("SGRHConnection"));
});

// Hotel Module

builder.Services.AddRoomDependency();
builder.Services.AddRatesDependency();

builder.Services.AddScoped<IRatesRepository, RatesRepository>();
builder.Services.AddTransient<IRatesService, RatesService>();
builder.Services.AddTransient<IRateMapper, RateMapper>();

builder.Services.AddScoped<IFloorRepository, FloorRepository>();
builder.Services.AddTransient<IFloorService, FloorService>();

builder.Services.AddScoped<IRoomCategoryRepository, RoomCategoryRepository>();
builder.Services.AddTransient<IRoomCategoryService, RoomCategoryService>();

// Report Module

builder.Services.AddReportDependency();

// Reservations 

builder.Services.AddReservationDependency();
builder.Services.AddReservationServiceDependency();

// Service Module
builder.Services.AddServiceDependency();


// User Management Module
builder.Services.AddUserManagementRepositories(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddUserManagementRepositories(builder.Configuration);

// Http Related

builder.Services.AddHttpClient<IHttpClientService, HttpClientService>();

builder.Services.AddTransient<IRateEndpoints, RateEndpoints>();
builder.Services.AddTransient<IRoomEndpoints, RoomEndpoints>();

builder.Services.AddScoped<IRateApiRepository, RateApiRepository>();
builder.Services.AddScoped<IRoomApiRepository, RoomApiRepository>();

// Logger

builder.Services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
