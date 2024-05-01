using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystemAPI.Configuration
{
    public sealed class MessageBrokerOptions
    {

        public required string HostName { get; set; }

        public required string CreateProductQueueName { get; set; }
        public required string UpdateProductQueueName { get; set; }
        public required string DeleteProductQueueName { get; set; }
        public required string ListProductQueueName { get; set; }

        public required string CreateOrderQueueName { get; set; }
        public required string UpdateOrderQueueName { get; set; }
        public required string DeleteOrderQueueName { get; set; }
        public required string ListOrderQueueName { get; set; }


    }
}
