using System;
using EventBus.Core;

namespace EventDrivenApp.Events
{
    /// <summary>
    /// Event triggered when a new user is registered.
    /// </summary>
    public class UserRegisteredEvent : IEvent
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime OccurredOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The type of the event (e.g., "UserRegistered").
        /// </summary>
        public string EventType { get; set; } = nameof(UserRegisteredEvent);

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
}
