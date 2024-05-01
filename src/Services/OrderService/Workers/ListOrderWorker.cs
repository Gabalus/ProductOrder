using System.Text;
using MediatR;
using MessageBroker.Abstracts;
using Microsoft.Extensions.Options;
using OrderCommon.Commands;
using OrderCommon.Entities;
using OrderService.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceCommon;

namespace OrderService.Workers;

public class ListOrderWorker : BackgroundService
{
    private readonly ILogger<ListOrderWorker> _logger;


    public ListOrderWorker(ILogger<ListOrderWorker> logger, IOptions<MessageBrokerOptions> options, IRPCServer rpcServer, ISender sender)
    {
        _logger = logger;

        rpcServer.Start<ListOrderCommand, ServiceResult<OrderDto[]>>(options.Value.HostName, options.Value.ListOrderQueueName,  (cp) =>
        {

            return sender.Send(cp);

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
