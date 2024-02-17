using MediatR;
using Microsoft.AspNetCore.Components;

namespace Minerva.Application.Common;
internal class NotificationsBroker<T> : INotificationsBroker<T> where T : INotification
{
    private readonly HashSet<ISubscription> subscriptions = new();

    public void Dispose() => subscriptions.Clear();
    public Task Handle(T notification, CancellationToken cancellationToken)
    {
        return Task.WhenAll(subscriptions.Select(s => s.NotifyAsync(notification, cancellationToken)));
    }
    public IDisposable Subscribe(EventCallback<T> eventCallback)
    {
        var subscription = new SimpleSubscription<T>(this, eventCallback);
        subscriptions.Add(subscription);

        return subscription;
    }

    private class SimpleSubscription<TNotification>(NotificationsBroker<TNotification> owner, EventCallback<TNotification> callback) : ISubscription, IDisposable where TNotification : INotification
    {
        public void Dispose() => owner.subscriptions.Remove(this);
        public Task NotifyAsync(INotification notification, CancellationToken cancellationToken) => callback.InvokeAsync((TNotification)notification);
    }
}
