using MessageBroker.Abstracts;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace MessageBroker.Concretes
{


    public class RabbitRPCServer : IRPCServer
    {

  
        public void Start<TRequest, TResponse>(string hostName, string queueName, Func<TRequest, Task<TResponse>> func)
        {
            var factory = new ConnectionFactory { HostName = hostName };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);
            Console.WriteLine(" [x] Awaiting RPC requests");

            consumer.Received += async (model, ea) =>
            {


                var body = ea.Body.ToArray();
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;
                TResponse response = default;
                try
                {
                    var message = Encoding.UTF8.GetString(body);
                    TRequest request = JsonSerializer.Deserialize<TRequest>(message);

                    response = await func(request);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {

                    var responseBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));
                    channel.BasicPublish(exchange: string.Empty,
                                         routingKey: props.ReplyTo,
                                         basicProperties: replyProps,
                                         body: responseBytes);
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };
        }

       
    }
}