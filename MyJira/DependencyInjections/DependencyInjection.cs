using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyJira.Entity.Entities;
using MyJira.Infastructure.Context;
using MyJira.Repository.MemberRepository;
using MyJira.Repository.ProjectMemberRepository;
using MyJira.Repository.ProjectRepository;
using MyJira.Repository.TicketBoardRepository;
using MyJira.Repository.TicketRepository;
using MyJira.Services.AccountService;
using MyJira.Services.MemberService;
using MyJira.Services.ProjectMemberService;
using MyJira.Services.ProjectService;
using MyJira.Services.RoleService;
using MyJira.Services.TicketBoardService;
using MyJira.Services.TicketService;
using System.Text;

namespace MyJira.DependencyInjections
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<ITicketBoardRepository, TicketBoardRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IProjectMemberRepository, ProjectMemberRepository>();

            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITicketBoardService, TicketBoardService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IProjectMemberService, ProjectMemberService>();
            services.AddScoped<IMemberService, MemberService>();    
            services.AddScoped<IRoleService, RoleService>();

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
.AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });
            return services;
        }

        public static IServiceCollection AddJwtToken(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["Key"]))
                };
            });

            services.AddAuthorization();

            return services;

        }
    }
}
