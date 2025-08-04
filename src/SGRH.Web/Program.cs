// File: C:\Users\ander\Source\Repos\SGRH-admin-api\src\SGRH.Web\Program.cs

using SGRH.Web.Interfaces;
using SGRH.Web.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// =================================================================================================
// SOLUTION FOR HTTPCLIENT AND DEPENDENCY INJECTION ERRORS
// =================================================================================================
// 1. Register a typed HttpClient to be injected.
// 2. Associate the IClientService interface with the concrete ClientService implementation.
//    This allows the ClientsController to receive an instance of ClientService.
builder.Services.AddHttpClient<IClientService, ClientService>();
// =================================================================================================

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
