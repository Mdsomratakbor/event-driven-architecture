using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Core
{
    public interface IEvent
    {
        Guid Id { get; }
        DateTime OccurredOn { get; }
        string EventType { get; }
        string Source { get; }
        string CorrelationId { get; }
        string? TenantId { get; }
    }

}
