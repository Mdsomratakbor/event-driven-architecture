using EventBus.Core;
using EventBus.InMemory;
using EventBus.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.Extension;

public class EventBusBuilder
{
    private readonly IServiceCollection _services;

    public EventBusBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public EventBusBuilder UseRabbitMq(Action<RabbitMqOptions> configure)
    {
        var options = new RabbitMqOptions();
        configure(options);
        _services.AddSingleton(options);
        _services.AddSingleton<RabbitMqEventBus>();
        _services.AddSingleton<IEventBus>(sp => sp.GetRequiredService<RabbitMqEventBus>());
        return this;
    }

    public EventBusBuilder UseInMemory()
    {
        _services.AddSingleton<InMemoryEventBus>();
        _services.AddSingleton<IEventBus>(sp => sp.GetRequiredService<InMemoryEventBus>());
        return this;
    }
}
