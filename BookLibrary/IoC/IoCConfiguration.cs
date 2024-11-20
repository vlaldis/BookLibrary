using BookLibrary.DataLayer;
using BookLibrary.Services;
using BookLibrary.Quartz;
using BookLibrary.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.IoC;

public static class IoCConfiguration
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IBookService, BookService>();
        services.AddDbContext<IBookLibraryContext, BookLibraryContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddQuartz();
        return services;
    }
}
