using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Core;


public abstract class EventHandlerWrapper
{
    public abstract Task Handle(IEvent @event);
}

public class EventHandlerWrapper<TEvent> : EventHandlerWrapper where TEvent : IEvent
{
    private readonly IEventHandler<TEvent> _handler;

    public EventHandlerWrapper(IEventHandler<TEvent> handler)
    {
        _handler = handler;
    }

    public override async Task Handle(IEvent @event)
    {
        await _handler.HandleAsync((TEvent)@event);
    }
}

