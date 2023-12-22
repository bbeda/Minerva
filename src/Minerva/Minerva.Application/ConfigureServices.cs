using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Minerva.Application.Infrastructure;

namespace Minerva.Application;
public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(IServiceCollection services)
    {
        _ = services.AddDbContext<DataContext>(options =>
        {
            _ = options.UseNpgsql(builder =>
            {

            });
        });

        return services;
    }
}
