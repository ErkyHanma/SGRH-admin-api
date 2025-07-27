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
using SGRH.Persistence.Repositories.UserManagement;

using SGRH.IOC.Dependencies.ReservationModule;
using SGRH.IOC.Dependencies.ServiceModule;
using SGRH.IOC.Dependencies.Hotel;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddUserManagementRepositories(builder.Configuration);





// Load .env variables
//Env.Load();
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
builder.Configuration["ConnectionStrings:SGRHConnection"] = connectionString;

// Logger
builder.Services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));

// Validators
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

// Dependencias de Floor
builder.Services.AddFloorDependency();

// Dependencias de RoomCategory
builder.Services.AddRoomCategoryDependency();



builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddTransient<IRoomService, RoomService>();

builder.Services.AddScoped<IRatesRepository, RatesRepository>();
builder.Services.AddTransient<IRatesService, RatesService>();
builder.Services.AddTransient<IRateMapper, RateMapper>();

// Report Module
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddTransient<IReportService, ReportService>();

// Reservation Module

builder.Services.AddReservationDependency();
builder.Services.AddReservationServiceDependency();

// Service Module
builder.Services.AddServiceDependency();


// User Management Module
builder.Services.AddUserManagementRepositories(builder.Configuration);









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