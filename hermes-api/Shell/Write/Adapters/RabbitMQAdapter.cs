using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Hermes.Shell
{
    public partial class DomainInterpreter
    {
        void SendMessage<T>(string exchange, string routingKey, T message)
        {
            using (var connection = _rabbitMQConnection.CreateConnection()) {
                using (var channel = connection.CreateModel()) {
                    channel.ExchangeDeclare(exchange, type: ExchangeType.Fanout);
                    var msg = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(msg);
                    channel.BasicPublish(
                        exchange,
                        routingKey,
                        basicProperties: null,
                        body);
                }
            }
        }
    }
}