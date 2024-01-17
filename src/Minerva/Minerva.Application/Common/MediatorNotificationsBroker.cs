﻿using MediatR;
using Microsoft.AspNetCore.Components;

namespace Minerva.Application.Common;
internal class MediatorNotificationsBroker : INotificationHandler<INotification>, INotificationsBroker
{
    private readonly List<ISubscription> subscriptions = new();

    public void Dispose() => throw new NotImplementedException();
    public Task Handle(INotification notification, CancellationToken cancellationToken)
    {
        return Task.WhenAll(subscriptions.Select(s => s.NotifyAsync(notification, cancellationToken)));
    }
    public IDisposable Subscribe<T>(EventCallback<T> eventCallback) where T : INotification
    {
        var subscription = new SimpleSubscription<T>(this, eventCallback);
        subscriptions.Add(subscription);

        return subscription;
    }

    private class SimpleSubscription<T>(MediatorNotificationsBroker owner, EventCallback<T> callback) : ISubscription, IDisposable where T : INotification
    {
        public void Dispose() => owner.subscriptions.Remove(this);
        public Task NotifyAsync(INotification notification, CancellationToken cancellationToken) => callback.InvokeAsync((T)notification);
    }
}