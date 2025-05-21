using System;
using System.Collections.Concurrent;
using EventBus.Core;

namespace EventBus.InMemory;

public class InMemoryEventBus : IEventBus
{
    private readonly ConcurrentDictionary<Type, List<EventHandlerWrapper>> _handlers = new();

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
    {
        var eventType = typeof(TEvent);
        if (_handlers.TryGetValue(eventType, out var handlers))
        {
            foreach (var handler in handlers)
            {
                await handler.Handle(@event);
            }
        }
    }

    public void Subscribe<TEvent, THandler>()
        where TEvent : IEvent
        where THandler : IEventHandler<TEvent>, new()
    {
        var eventType = typeof(TEvent);
        var wrapper = new EventHandlerWrapper<TEvent>(new THandler());

        _handlers.AddOrUpdate(
            eventType,
            (_) => new List<EventHandlerWrapper> { wrapper },
            (_, existing) =>
            {
                existing.Add(wrapper);
                return existing;
            });
    }
}
