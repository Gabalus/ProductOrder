using System.Text;
using MediatR;
using MessageBroker.Abstracts;
using Microsoft.Extensions.Options;
using ProductCommon.Commands;
using ProductCommon.Entities;
using ProductService.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ProductService.Workers;

public class DeleteProductWorker : BackgroundService
{
    private readonly ILogger<DeleteProductWorker> _logger;


    public DeleteProductWorker(ILogger<DeleteProductWorker> logger, IOptions<MessageBrokerOptions> options, IRPCServer rpcServer, ISender sender)
    {
        _logger = logger;

        rpcServer.Start<DeleteProductCommand, object>(options.Value.HostName, options.Value.DeleteProductQueueName, async (cp) =>
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
