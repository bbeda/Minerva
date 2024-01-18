using MediatR;
using Microsoft.AspNetCore.Components;

namespace Minerva.Application.Common;
internal class MediatorNotificationsBroker<T> : INotificationsBroker<T> where T : INotification
{
    private readonly List<ISubscription> subscriptions = new();

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

    private class SimpleSubscription<T>(MediatorNotificationsBroker<T> owner, EventCallback<T> callback) : ISubscription, IDisposable where T : INotification
    {
        public void Dispose() => owner.subscriptions.Remove(this);
        public Task NotifyAsync(INotification notification, CancellationToken cancellationToken) => callback.InvokeAsync((T)notification);
    }
}
