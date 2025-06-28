using FluentValidation;
using SGRH.Application.Common.Logging;

using SGRH.Application.Dtos.Hotel.Floor;
using SGRH.Application.Dtos.Hotel.Floor.Validators;
using SGRH.Application.Dtos.Hotel.RoomCategory;
using SGRH.Application.Dtos.Hotel.RoomCategory.Validators;


using SGRH.Application.Interfaces.Repositories.Hotel;

using SGRH.Application.Interfaces.Services.Hotel;

using SGRH.Application.Services.Hotel;

using SGRH.Persistence.Repositories.Hotel;


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
            
            ////////////////////////////////// comentado de mientras
            //builder.Services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));

            // Validadores para Floor
            builder.Services.AddScoped<IValidator<CreateFloorDto>, CreateFloorValidator>();
            builder.Services.AddScoped<IValidator<ModifyFloorDto>, ModifyFloorValidator>();
            builder.Services.AddScoped<IValidator<DisableFloorDto>, DisableFloorValidator>();

            // Validadores para RoomCategor
            builder.Services.AddScoped<IValidator<CreateRoomCategoryDto>, CreateRoomCategoryValidator>();
            builder.Services.AddScoped<IValidator<ModifyRoomCategoryDto>, ModifyRoomCategoryValidator>();
            builder.Services.AddScoped<IValidator<DisableRoomCategoryDto>, DisableRoomCategoryValidator>();


            // Modulo de Pisos (Floor)
            builder.Services.AddScoped<IFloorRepository, FloorRepository>();
            builder.Services.AddTransient<IFloorService, FloorService>();

            // Modulo de Categorias de Habitaciones (RoomCategory)
            builder.Services.AddScoped<IRoomCategoryRepository, RoomCategoryRepository>();
            builder.Services.AddTransient<IRoomCategoryService, RoomCategoryService>();



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