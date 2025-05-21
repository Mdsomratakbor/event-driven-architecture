using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Core
{
    public enum EventBusType
    {
        InMemory,
        RabbitMQ
    }
    public enum ExchangeTypeEnum
    {
        Direct,
        Fanout,
        Topic,
        Headers
    }

}
