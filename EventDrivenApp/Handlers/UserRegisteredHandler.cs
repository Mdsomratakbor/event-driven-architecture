using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBus.Core;
using EventDrivenApp.Events;

namespace EventDrivenApp.Handlers
{
    public class UserRegisteredHandler : IEventHandler<UserRegisteredEvent>
    {
        public Task HandleAsync(UserRegisteredEvent @event)
        {
            Console.WriteLine("[UserHandler] Welcome email sent to");
            Console.WriteLine($"  Id: {@event.Id}");
            Console.WriteLine($"  OccurredOn: {@event.OccurredOn}");
            Console.WriteLine($"  CustomerEmail: {@event.Email}");
            Console.WriteLine($"  EventType: {@event.EventType}");
            Console.WriteLine($"  Source: {@event.Source}");
            Console.WriteLine($"  CorrelationId: {@event.CorrelationId}");
            Console.WriteLine($"  TenantId: {@event.TenantId ?? "N/A"}");
            return Task.CompletedTask;
        }
    }

}
