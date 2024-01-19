using MediatR;
using Microsoft.AspNetCore.Components;

namespace Minerva.Application.Common;
public interface INotificationsBroker<T> : IDisposable, INotificationHandler<T> where T : INotification
{
    IDisposable Subscribe(EventCallback<T> eventCallback);
}

public interface ISubscription : IDisposable
{
    public Task NotifyAsync(INotification notification, CancellationToken cancellationToken);
}
