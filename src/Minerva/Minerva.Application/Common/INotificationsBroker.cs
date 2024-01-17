using MediatR;
using Microsoft.AspNetCore.Components;

namespace Minerva.Application.Common;
public interface INotificationsBroker : IDisposable,INotificationHandler<INotification>
{
    IDisposable Subscribe<T>(EventCallback<T> eventCallback) where T : INotification;
}

public interface ISubscription : IDisposable
{
    public Task NotifyAsync(INotification notification, CancellationToken cancellationToken);
}
