using MyJira.Repository.ProjectRepository;
using MyJira.Repository.TicketRepository;
using MyJira.Services.ProjectService;

namespace MyJira.DependencyInjections
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();

            services.AddScoped<IProjectService, ProjectService>();
           // services.AddScoped<ITicketSe>
           return services;
        }
    }
}
