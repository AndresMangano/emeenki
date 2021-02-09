using System;
using System.Text;
using System.Threading;
using Hermes.Worker.Core.Ports;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Hermes.Worker.Shell
{
    public class RabbitMQHandler : IRabbitMQHandler
    {
        readonly IModel _channel;
        public RabbitMQHandler(IModel channel) {
            _channel = channel;
        }

        public void DeclareRoute(string queue, string exchange, DefaultConsumer consumer)
        {
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
                    Console.WriteLine($"Message received => {queue}:{ea.RoutingKey}");
                    consumer(ea.RoutingKey, message);
                    _channel.BasicAck(ea.DeliveryTag, false);
                } catch (Exception ex) {
                    Console.WriteLine($"Exception => {ex.Message}");
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
            var factory = new ConnectionFactory { HostName = _settings.RabbitMQ };
            while (retries > 0) {
                try {
                    using (var connection = factory.CreateConnection()) {
                        using (var channel = connection.CreateModel()) {
                            model(new RabbitMQHandler(channel));
                            Console.WriteLine("Listening messages...");
                            retries = 0;
                            Thread.Sleep(Timeout.Infinite);
                        }
                    }
                } catch(Exception ex) {
                    Console.WriteLine(ex.Message);
                    retries--;
                    Thread.Sleep(5000);
                }
            }
        }
    }
}