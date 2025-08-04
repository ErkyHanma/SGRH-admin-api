using SGRH.Application.Common.Logging;
using SGRH.Common.Common;
using SGRH.Web.Interfaces.HttpClients;
using SGRH.Web.IOC.HttpClient;
using SGRH.Web.Services.HttpClients;


var builder = WebApplication.CreateBuilder(args);

// Configuration
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
builder.Configuration["ConnectionStrings:SGRHConnection"] =
    "Host=aws-0-us-east-2.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.vcnsurdtnpeycpifqzxl;Password=Erkyhanma002;SSL Mode=Require;Trust Server Certificate=true";

var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];

// Http clients
builder.Services.AddHttpClientDependencies();

builder.Services.AddHttpClient<IBaseHttpClientMethods, BaseHttpClientMethods>(client =>
{
    client.BaseAddress = new Uri(baseUrl);
});

// Logger
builder.Services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));

// MVC & dependencies
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
