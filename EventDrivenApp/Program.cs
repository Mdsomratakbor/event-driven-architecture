using EventBus.Core;
using EventBus.Extension;
using EventDrivenApp.Events;
using EventDrivenApp.Handlers;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Reflection;



var services = new ServiceCollection();


services.AddEventBus(builder =>
{
    builder.UseRabbitMq(opts =>
    {
        opts.HostName = "localhost";
        opts.ExchangeName = "my_exchange";
        opts.ExchangeType = ExchangeTypeEnum.Topic.ToString().ToLower(); 
        opts.DurableExchange = true;
        opts.QueueNamePrefix = "app";
        opts.RoutingKey = "order.*";
        opts.AutoAck = false;
    });
});


// Auto-register all event handlers from the assembly containing your handlers
//services.AddEventHandlersFromAssembly(typeof(OrderCreatedHandler).Assembly);

// Build the service provider
var serviceProvider = services.BuildServiceProvider();

// Resolve the event bus
var eventBus = serviceProvider.GetRequiredService<IEventBus>();

// Subscribe event handlers explicitly (this is runtime wiring)
eventBus.Subscribe<OrderCreatedEvent, OrderCreatedHandler>();
eventBus.Subscribe<OrderCreatedEvent, OrderAnalyticsHandler>();
eventBus.Subscribe<UserRegisteredEvent, UserRegisteredHandler>();

// Publish test events
await eventBus.PublishAsync(new OrderCreatedEvent
{
    OrderId = "ORD-RABBIT-001",
    CustomerEmail = "rabbit@example.com"
});

await eventBus.PublishAsync(new UserRegisteredEvent
{
    UserId = "USR001",
    Email = "user@example.com"
});

Console.WriteLine("Events Published.");
Console.ReadLine();
