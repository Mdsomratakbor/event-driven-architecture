using EventBus.Core;

namespace EventDrivenApp.Events;

public class OrderCreatedEvent : IEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public string OrderId { get; set; } = "";
    public string CustomerEmail { get; set; } = "";
    /// <summary>
    /// The type of the event (e.g., "UserRegistered").
    /// </summary>
    public string EventType { get; set; } = nameof(OrderCreatedEvent);

    /// <summary>
    /// The source system or application that emitted the event.
    /// </summary>
    public string Source { get; set; } = "EventDrivenApp";

    /// <summary>
    /// Used to correlate this event with other related events or requests.
    /// </summary>
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Optional: Tenant identifier for multi-tenant systems.
    /// </summary>
    public string? TenantId { get; set; }
}
