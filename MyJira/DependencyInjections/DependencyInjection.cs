using Microsoft.AspNetCore.Identity;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Context;
using MyJira.Repository.ProjectRepository;
using MyJira.Repository.TicketBoardRepository;
using MyJira.Repository.TicketRepository;
using MyJira.Services.ProjectService;
using MyJira.Services.TicketBoardService;
using MyJira.Services.TicketService;

namespace MyJira.DependencyInjections
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<ITicketBoardRepository, TicketBoardRepository>();

            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITicketBoardService, TicketBoardService>();
            services.AddScoped<ITicketService, TicketService>();
           
           return services;
        }

        public static IServiceCollection AddMyIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            services.AddControllersWithViews();

            return services;
        }
    }
}
