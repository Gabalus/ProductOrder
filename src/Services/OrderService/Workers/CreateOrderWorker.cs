

using System.Text;
using MediatR;
using MessageBroker.Abstracts;
using Microsoft.Extensions.Options;
using OrderCommon.Commands;
using OrderCommon.Entities;
using OrderService.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace OrderService.Workers;

public class CreateOrderWorker : BackgroundService
{
    private readonly ILogger<CreateOrderWorker> _logger;


    public CreateOrderWorker(ILogger<CreateOrderWorker> logger, IOptions<MessageBrokerOptions> options, IRPCServer rpcServer, ISender sender)
    {
        _logger = logger;

        rpcServer.Start<CreateOrderCommand, ServiceResult<OrderDto>>(options.Value.HostName, options.Value.CreateOrderQueueName, async (cp) =>
        {

            return await sender.Send(cp);

        });



    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            await Task.Delay(1000, stoppingToken);
        }
    }
}
