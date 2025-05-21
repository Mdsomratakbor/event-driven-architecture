using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBus.Core;
using RabbitMQ.Client;

namespace EventBus.RabbitMQ
{
    public class RabbitMqOptions
    {
        public string HostName { get; set; } = "localhost";
        public string ExchangeName { get; set; } = "event_bus";
        public string ExchangeType { get; set; } = ExchangeTypeEnum.Fanout.ToString(); // "fanout", "direct", "topic", etc.
        public bool DurableExchange { get; set; } = true;
        public bool DurableQueue { get; set; } = true;
        public bool AutoDeleteQueue { get; set; } = false;
        public string QueueNamePrefix { get; set; } = "";
        public string RoutingKey { get; set; } = "";
        public bool AutoAck { get; set; } = false;
        public IBasicProperties? BasicProperties { get; set; } = null; // Optional for custom message properties
    }

}
