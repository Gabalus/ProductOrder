using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Net.Sockets;
using System.Threading.Channels;
using System.Collections.Concurrent;
using System.Text;
using MessageBroker.Abstracts;
using OrderCommon.Commands;
using OrderCommon.Entities;
using System.Text.Json;
using Microsoft.Extensions.Options;
using OrderSystemAPI.Configuration;
using ServiceCommon;

namespace OrderSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly ILogger<OrderController> _logger;
        private readonly IRPCClient _rpcClient;
        private readonly MessageBrokerOptions _options;

        public OrderController(ILogger<OrderController> logger, IRPCClient rpcClient, IOptions<MessageBrokerOptions> options)
        {
            _logger = logger;
            _rpcClient = rpcClient;
            _options = options.Value;

        }

        [HttpPost]        
        public async Task<ServiceResult<OrderDto>> Post(CreateOrderCommand create)
        {

            var newOrder = await _rpcClient.GetResponse<CreateOrderCommand, OrderDto>(_options.HostName, _options.CreateOrderQueueName, create);
            return newOrder;

        }

        [HttpPatch()]
        public async Task<ServiceResult<OrderDto>> Patch(UpdateOrderCommand update)
        {

            var updatedOrder = await _rpcClient.GetResponse<UpdateOrderCommand, OrderDto>(_options.HostName, _options.UpdateOrderQueueName, update);
            return updatedOrder;

        }

        [HttpDelete()]
        public async Task<ServiceResult> Delete(DeleteOrderCommand delete)
        {

         return   await _rpcClient.GetResponse<DeleteOrderCommand, object>(_options.HostName, _options.DeleteOrderQueueName, delete);


        }

        [HttpGet()]
        public async Task<ServiceResult<OrderDto[]>> List(int count, int page)
        {

            var Orders = await _rpcClient.GetResponse<ListOrderCommand, OrderDto[]>(_options.HostName, _options.ListOrderQueueName, new ListOrderCommand() { Count = count, Page = page });
            return Orders;

        }





    }
}