using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyJira.DependencyInjections;
using MyJira.Infastructure.Context;
using MyJira.Infastructure.Mapper;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var solutionDir = Directory.GetParent(builder.Environment.ContentRootPath)!.FullName;
var logPath = Path.Combine(solutionDir, "logs", "log-.log");
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MyJiraConnection")));
builder.Services.AddAutoMapper(typeof(MappingProfile));
//builder.Services.AddAutoMapper(MappingProfile);
Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
        path: logPath,     
        rollingInterval: RollingInterval.Day, 
        retainedFileCountLimit: 50,         
        shared: true
    )
    .CreateLogger();
builder.Services.AddProjectServices();
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
    pattern: "{controller=Project}/{action=Index}/{id?}");

app.Run();
