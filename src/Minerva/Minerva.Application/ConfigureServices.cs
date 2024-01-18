using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minerva.Application.Common;
using Minerva.Application.Features.TaskItems;
using Minerva.Application.Infrastructure;

namespace Minerva.Application;
public static class ConfigureServices
{
    private const string EntitiesStoreConnectionString = "EntitiesStore";

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDbContext<DataContext>(options =>
        {
            _ = options.UseNpgsql(configuration.GetConnectionString(EntitiesStoreConnectionString));
        });

        _ = services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ConfigureServices).Assembly));

        _ = services.AddScoped<ITaskItemRepository, TaskItemRepository>();

        _ = services.AddScoped<IUnitOfWork>(sp =>
        {
            return sp.GetRequiredService<DataContext>();
        });

        services.AddScoped(typeof(INotificationHandler<TaskItemCompletedNotification>), sc => sc.GetRequiredService(typeof(INotificationsBroker<TaskItemCompletedNotification>)));

        services.AddSingleton(typeof(INotificationsBroker<>), typeof(MediatorNotificationsBroker<>));

        return services;
    }
}
