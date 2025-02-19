using LibraryManagementSystem.Infrastructure.Repositories.Implementation;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagementSystem.Infrastructure
{
    public static class ModuleInfrastructureDependencies
    {
        public static void AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IBorrowingRepository, BorrowingRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        }
    }
}
