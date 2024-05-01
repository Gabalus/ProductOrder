using MessageBroker.Abstracts;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Text.Json;
using ServiceCommon;

namespace MessageBroker.Concretes
{

    public class RabbitRPCClient : IRPCClient
    {
      

        public async Task<string> GetResponseAsString<TRequest, TResponse>(string hostname, string queueName, TRequest request)
        {
            var factory = new ConnectionFactory { HostName = hostname };

            ConcurrentDictionary<string, TaskCompletionSource<string>> callbackMapper = new();

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            // declare a server-named queue
            var replyQueueName = channel.QueueDeclare().QueueName;
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                if (!callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                    return;
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                tcs.TrySetResult(response);
            };

            channel.BasicConsume(consumer: consumer,
                                 queue: replyQueueName,
                                 autoAck: true);


            IBasicProperties props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;
            var messageBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(request));
            var tcs = new TaskCompletionSource<string>();
            callbackMapper.TryAdd(correlationId, tcs);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: queueName,
                                 basicProperties: props,
                                 body: messageBytes);

            return await tcs.Task; 



        }

        public async Task<ServiceResult<TResponse>> GetResponse<TRequest, TResponse>(string hostname, string queueName, TRequest request)
        {

            var response = await GetResponseAsString<TRequest, TResponse>(hostname, queueName, request);

            return JsonSerializer.Deserialize<ServiceResult<TResponse>>(response);
        }
    }
}
