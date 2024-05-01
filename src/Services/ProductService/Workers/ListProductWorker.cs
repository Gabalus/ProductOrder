using System.Text;
using MediatR;
using MessageBroker.Abstracts;
using Microsoft.Extensions.Options;
using ProductCommon.Commands;
using ProductCommon.Entities;
using ProductService.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceCommon;

namespace ProductService.Workers;

public class ListProductWorker : BackgroundService
{
    private readonly ILogger<ListProductWorker> _logger;


    public ListProductWorker(ILogger<ListProductWorker> logger, IOptions<MessageBrokerOptions> options, IRPCServer rpcServer, ISender sender)
    {
        _logger = logger;

        rpcServer.Start<ListProductCommand, ServiceResult<ProductDto[]>>(options.Value.HostName, options.Value.ListProductQueueName,  (cp) =>
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
