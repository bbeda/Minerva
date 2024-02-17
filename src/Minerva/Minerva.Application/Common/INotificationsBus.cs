using MediatR;
using Microsoft.AspNetCore.Components;

namespace Minerva.Application.Common;
public interface INotificationsBus<T> : IDisposable where T : INotification
{
    IDisposable Subscribe(EventCallback<T> eventCallback);

    Task PublishAsync(T notification, CancellationToken cancellationToken);
}

public interface INotificationSubscription : IDisposable
{
    public Task NotifyAsync(INotification notification, CancellationToken cancellationToken);
}
