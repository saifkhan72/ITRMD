using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RMDWEB.Data;
using Serilog;
using Serilog.Extensions.Hosting;
using System.Configuration;
using RMDWEB.Models;
using RMDWEB.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, config) =>
{

    if(!Directory.Exists("logs"))
    {
        Directory.CreateDirectory("logs");
    }

    config.MinimumLevel.Information()
     .WriteTo.File("logs/info-.txt", rollingInterval: RollingInterval.Day,
     retainedFileCountLimit: 30, shared: true, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {RequestId}{Message:lj}{NewLine}{Exception}");

    config.MinimumLevel.Error()
        .WriteTo.File("logs/error-.txt", rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30, shared: true, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {RequestId}{Message:lj}{NewLine}{Exception}");

    config.MinimumLevel.Debug()
      .WriteTo.File("logs/debug-.txt", rollingInterval: RollingInterval.Day,
      retainedFileCountLimit: 30, shared: true, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {RequestId}{Message:lj}{NewLine}{Exception}");

    config.MinimumLevel.Warning()
        .WriteTo.File("logs/warning-.txt", rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30, shared: true, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {RequestId}{Message:lj}{NewLine}{Exception}");



});



// Add services to the container.
var connectionString = builder.Configuration
        .GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount=false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews(x => x.SuppressAsyncSuffixInActionNames=false)
.AddRazorRuntimeCompilation();



var app = builder.Build();

//app.Environment.IsProduction();
app.Environment.IsDevelopment();
// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();


