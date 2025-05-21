using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBus.Core;
using EventDrivenApp.Events;

namespace EventDrivenApp.Handlers
{
    public class OrderAnalyticsHandler : IEventHandler<OrderCreatedEvent>
    {
        public Task HandleAsync(OrderCreatedEvent @event)
        {
            Console.WriteLine("[AnalyticsService] Tracking order ID");
            Console.WriteLine($"  Id: {@event.Id}");
            Console.WriteLine($"  OccurredOn: {@event.OccurredOn}");
            Console.WriteLine($"  OrderId: {@event.OrderId}");
            Console.WriteLine($"  CustomerEmail: {@event.CustomerEmail}");
            Console.WriteLine($"  EventType: {@event.EventType}");
            Console.WriteLine($"  Source: {@event.Source}");
            Console.WriteLine($"  CorrelationId: {@event.CorrelationId}");
            Console.WriteLine($"  TenantId: {@event.TenantId ?? "N/A"}");
            return Task.CompletedTask;
        }
    }

}
