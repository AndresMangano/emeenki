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

        public JObject ParseEvent<TKey, T>(long index, DomainEventMetadata<TKey> header, T payload, Action<JObject> appendID)
        {
            var msgHeader = JObject.FromObject(header);
            msgHeader.Add("Index", index);
            JObject msg = JObject.FromObject(payload);
            msg.Add("Header", msgHeader);
            appendID(msg);

            return msg;
        }

        void SendMessage<TKey, T>(string exchange, long index, DomainEventMetadata<TKey> header, T payload, Action<JObject> appendID)
        {
            using (var connection = _rabbitMQConnection.CreateConnection()) {
                using (var channel = connection.CreateModel()) {
                    channel.ExchangeDeclare(exchange, type: ExchangeType.Fanout);
                    var msg = ParseEvent(index, header, payload, appendID);
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