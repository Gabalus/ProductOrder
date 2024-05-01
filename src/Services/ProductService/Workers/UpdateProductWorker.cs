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

public class UpdateProductWorker : BackgroundService
{
    private readonly ILogger<UpdateProductWorker> _logger;


    public UpdateProductWorker(ILogger<UpdateProductWorker> logger, IOptions<MessageBrokerOptions> options, IRPCServer rpcServer, ISender sender)
    {
        _logger = logger;

        rpcServer.Start<UpdateProductCommand, ServiceResult<ProductDto>>(options.Value.HostName, options.Value.UpdateProductQueueName, async (cp) =>
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
