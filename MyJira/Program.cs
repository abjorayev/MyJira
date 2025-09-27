using Microsoft.EntityFrameworkCore;
using MyJira.Infastructure.Context;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var solutionDir = Directory.GetParent(builder.Environment.ContentRootPath)!.FullName;
var logPath = Path.Combine(solutionDir, "logs", "log-.log");
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MyJiraConnection")));

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
        path: logPath,     
        rollingInterval: RollingInterval.Day, 
        retainedFileCountLimit: 50,         
        shared: true
    )
    .CreateLogger();

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
