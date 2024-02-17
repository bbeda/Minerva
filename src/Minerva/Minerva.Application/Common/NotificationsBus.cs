using MediatR;
using Microsoft.AspNetCore.Components;

namespace Minerva.Application.Common;
internal class NotificationsBus<T> : INotificationsBus<T> where T : INotification
{
    private readonly HashSet<INotificationSubscription> subscriptions = new();

    public void Dispose() => subscriptions.Clear();
    public Task PublishAsync(T notification, CancellationToken cancellationToken)
    {
        return Task.WhenAll(subscriptions.Select(s => s.NotifyAsync(notification, cancellationToken)));
    }
    public IDisposable Subscribe(EventCallback<T> eventCallback)
    {
        var subscription = new SimpleSubscription<T>(this, eventCallback);
        subscriptions.Add(subscription);

        return subscription;
    }

    private class SimpleSubscription<TNotification>(NotificationsBus<TNotification> owner, EventCallback<TNotification> callback) : INotificationSubscription, IDisposable where TNotification : INotification
    {
        public void Dispose() => owner.subscriptions.Remove(this);
        public Task NotifyAsync(INotification notification, CancellationToken cancellationToken) => callback.InvokeAsync((TNotification)notification);
    }
}
