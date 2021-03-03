using System;
using System.Text;
using System.Threading;
using Hermes.Worker.Core.Ports;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Hermes.Worker.Shell
{
    public class RabbitMQHandler : IRabbitMQHandler
    {
        readonly IModel _channel;
        readonly ILogger<RabbitMQHandler> _logger;

        public RabbitMQHandler(IModel channel, ILoggerFactory loggerFactory) {
            _logger = loggerFactory.CreateLogger<RabbitMQHandler>();
            _channel = channel;
        }

        public void DeclareRoute(string queue, string exchange, DefaultConsumer consumer)
        {
            _logger.LogInformation("Declare queue and exchange {queue}:{exchange}",
                queue,
                exchange);

            _channel.ExchangeDeclare(
                exchange: exchange,
                type: ExchangeType.Fanout
            );
            _channel.QueueDeclare(
                queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            _channel.QueueBind(
                queue: queue,
                exchange: exchange,
                routingKey: ""
            );
            var basicConsumer = new EventingBasicConsumer(_channel);
            basicConsumer.Received += (model, ea) => {
                try {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    _logger.LogInformation("Message received => {queue}:{routingKey}",
                        queue,
                        ea.RoutingKey);
                    consumer(ea.RoutingKey, message);
                    _channel.BasicAck(ea.DeliveryTag, false);
                } catch (Exception ex) {
                    _logger.LogError(ex, "Failed to read message");
                }
            };
            _channel.BasicConsume(
                queue: queue,
                autoAck: false,
                consumer: basicConsumer
            );
        }
    }
    public partial class Interpreter : IRabbitMQPort
    {
        public void CreateModelAndWait(Action<IRabbitMQHandler> model)
        {
            var retries = 10;
            var factory = new ConnectionFactory {
                HostName = _settings.Queue.HostName,
                UserName = _settings.Queue.UserName,
                Password = _settings.Queue.Password
            };
            while (retries > 0) {
                try {
                    using (var connection = factory.CreateConnection()) {
                        _logger.LogInformation("RabbitMQ connection succeeded");
                        using (var channel = connection.CreateModel()) {
                            _logger.LogInformation("Create model");
                            model(new RabbitMQHandler(channel, _loggerFactory));
                            retries = 0;

                            _logger.LogInformation("Waiting messages");
                            Thread.Sleep(Timeout.Infinite);
                        }
                    }
                } catch(Exception ex) {
                    _logger.LogError(ex, "Failed to connect to RabbitMQ");

                    retries--;
                    Thread.Sleep(5000);
                }
            }
        }
    }
}