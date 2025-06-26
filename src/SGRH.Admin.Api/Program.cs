using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.Repositories.Report;
using SGRH.Application.Interfaces.Services.Hotel;
using SGRH.Application.Interfaces.Services.Report;
using SGRH.Application.Services.Hotel;
using SGRH.Application.Services.Report;
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

            // Modulo de habitaciones

            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddTransient<IRoomService, RoomService>();

            // Modulo de Reportes

            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddTransient<IReportService, ReportService>();

            // Modulo Tarifa (esperando)

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