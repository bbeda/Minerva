
using MediatR;
using Minerva.Application.Features.TaskItems;

namespace Minerva.WebApp.BackgroundServices;

public class PlanningWatcherService(
    TimeProvider timeProvider,
    IServiceProvider serviceProvider) : IHostedService, IDisposable
{
    private ITimer? timer;

    public void Dispose()
    {
        timer?.Dispose();
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        timer = timeProvider.CreateTimer(async (state) =>
        {
            using var scope = serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var command = new CheckAndUpdateTaskItemPlanStatusCommand();
            await mediator.Send(command);
        }, null, TimeSpan.FromSeconds(15), TimeSpan.FromMinutes(5));

        return Task.CompletedTask;
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        timer?.Change(TimeSpan.MaxValue, TimeSpan.MaxValue);
        return Task.CompletedTask;
    }
}
