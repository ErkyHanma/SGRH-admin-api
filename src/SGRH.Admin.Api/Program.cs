using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection; 
using SGRH.Persistence.Repositories.UserManagement; 


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddUserManagementRepositories(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Port change for CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:5171") // <<-- IMPORTANT! Adjust this port if your API uses a different one PD: Andersson
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
// --- END OFF CHANGES ---

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin"); // Apply the CORS policy defined above PD: Andrsson

app.UseAuthorization();

app.MapControllers();

app.Run();