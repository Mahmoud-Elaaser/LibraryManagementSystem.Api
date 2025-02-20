using LibraryManagementSystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LibraryManagementSystem.Application
{
    public static class ModuleApplicationDependencies
    {
        public static void AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
