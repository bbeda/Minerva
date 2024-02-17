using MediatR;
using Microsoft.AspNetCore.Components;

namespace Minerva.Application.Common;
public interface INotificationsBroker<T> : IDisposable where T : INotification
{
    IDisposable Subscribe(EventCallback<T> eventCallback);

    Task Handle(T notification, CancellationToken cancellationToken);
}

public interface ISubscription : IDisposable
{
    public Task NotifyAsync(INotification notification, CancellationToken cancellationToken);
}
