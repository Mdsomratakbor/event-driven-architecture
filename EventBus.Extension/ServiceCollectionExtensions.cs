using System.Reflection;
using EventBus.Core;
using EventBus.InMemory;
using EventBus.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.Extension;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventBus(this IServiceCollection services, Action<EventBusBuilder> configure)
    {
        var builder = new EventBusBuilder(services);
        configure(builder);
        return services;
    }

    public static IServiceCollection AddEventHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                .Select(i => new { HandlerType = t, InterfaceType = i }));

        foreach (var handler in handlerTypes)
        {
            services.AddTransient(handler.InterfaceType, handler.HandlerType);
        }

        return services;
    }
}
