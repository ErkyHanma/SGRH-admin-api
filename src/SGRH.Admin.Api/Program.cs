using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection; 
using SGRH.Persistence.Repositories.UserManagement; 


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddUserManagementRepositories(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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