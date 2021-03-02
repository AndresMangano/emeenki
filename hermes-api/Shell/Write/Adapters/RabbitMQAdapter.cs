using System;
using System.Text;
using Hermes.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;

namespace Hermes.Shell
{
    public partial class DomainInterpreter
    {
        void SendMessage<TKey, T>(string exchange, long index, DomainEventMetadata<TKey> header, T payload)
        {
            using (var connection = _rabbitMQConnection.CreateConnection()) {
                using (var channel = connection.CreateModel()) {
                    channel.ExchangeDeclare(exchange, type: ExchangeType.Fanout);
                    var msgHeader = JObject.FromObject(header);
                    msgHeader.Add("Index", index);
                    JObject msg = JObject.FromObject(payload);
                    msg.Add("Header", msgHeader);
                    var body = Encoding.UTF8.GetBytes(msg.ToString());
                    channel.BasicPublish(
                        exchange,
                        header.EventName,
                        basicProperties: null,
                        body);
                }
            }
        }
    }
}