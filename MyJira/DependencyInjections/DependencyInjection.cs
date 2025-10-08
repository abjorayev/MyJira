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
           // services.AddScoped<ITicketSe>
           return services;
        }
    }
}
