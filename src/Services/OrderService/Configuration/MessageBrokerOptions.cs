using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Configuration
{
    public sealed class MessageBrokerOptions
    {

        public required string HostName { get; set; }

        public required string CreateOrderQueueName { get; set; }
        public required string UpdateOrderQueueName { get; set; }
        public required string DeleteOrderQueueName { get; set; }
        public required string ListOrderQueueName { get; set; }
        

    }
}
