// File: SGRH.Web/Program.cs

using SGRH.Web.Interfaces;
using SGRH.Web.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// =================================================================================================
// SOLUCION FINAL PARA HTTPCLIENT Y DEPENDENCY INJECTION
// =================================================================================================
// 1. Registra un HttpClient tipado para IClientService.
// 2. Asociamos la interfaz IClientService con la implementación ClientService.
// 3. Configuramos la BaseAddress del HttpClient de forma centralizada.
builder.Services.AddHttpClient<IClientService, ClientService>(client =>
{
    // Obtenemos la URL base desde la configuración del proyecto (appsettings.json).
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiSettings:BaseUrl") ??
                                 throw new InvalidOperationException("ApiSettings:BaseUrl not found in configuration."));
});
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
