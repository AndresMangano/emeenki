using System;

namespace Hermes.Worker.Core.Model
{
    public sealed class DefaultMessage<TKey, TPayload>
    {
        public long Index { get; }
        public DefaultMessageBody<TKey, TPayload> Message { get; }
        public DefaultMessage(long index, DefaultMessageBody<TKey, TPayload> message)
        {
            Index = index;
            Message = message;
        }
    }
    public class DefaultMessageBody<TKey, TPayload>
    {
        public DefaultMessageMetadata<TKey> Metadata { get; }
        public TPayload Payload { get; }
        public DefaultMessageBody(DefaultMessageMetadata<TKey> metadata, TPayload payload)
        {
            Metadata = metadata;
            Payload = payload;
        }
    }
    public class DefaultMessageMetadata<TKey>
    {
        public TKey ID { get; }
        public int Version { get; }
        public DateTime Timestamp { get; }
        public string Stream { get; }
        public string EventName { get; }
        public DefaultMessageMetadata(TKey id, int version, DateTime timestamp, string stream, string eventName) {
            ID = id;
            Version = version;
            Timestamp = timestamp;
            Stream = stream;
            EventName = eventName;
        }
    }
}