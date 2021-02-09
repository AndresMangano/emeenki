using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Hermes.Core;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Hermes.Shell
{
    public sealed class SQLEventStorage<TKey> : IEventStorage<TKey>
    {
        private class EventModel
        {
            public long Seq { get; set; }
            public TKey ID { get; set; }
            public int Version { get; set; }
            public string EventName { get; set; }
            public DateTime Timestamp { get; set; }
            public string Payload { get; set; }
        }
        private readonly string _connectionString;
        private readonly string _streamName;
        private readonly Func<string, string, object> _pharser;

        public SQLEventStorage(string connectionString, string streamName, Func<string, string, object> pharser)
        {
            _connectionString = connectionString;
            _streamName = streamName;
            _pharser = pharser;
        }

        public IEnumerable<DomainEvent<TKey, object>> GetOrderedEvents(TKey ID)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString)) {
                conn.Open();
                return conn.Query<EventModel>($@"
                    SELECT * FROM {_streamName}_Events WHERE ID = @ID
                    ORDER BY Version ASC", new { ID = ID }).Select(em => new DomainEvent<TKey, object>(
                        id: em.ID,
                        version: em.Version,
                        stream: _streamName,
                        eventName: em.EventName,
                        timestamp: em.Timestamp,
                        payload: _pharser(em.EventName, em.Payload)
                    ));
            }
        }

        public EventMessage<TKey, object> StoreEvent(DomainEvent<TKey, object> evnt)
        {
            using(MySqlConnection conn = new MySqlConnection(_connectionString)) {
                conn.Open();
                using(MySqlCommand cmd = conn.CreateCommand()){
                    cmd.CommandText = $@"
                        INSERT INTO {_streamName}_Events(ID, Version, EventName, `Timestamp`, Payload)
                        VALUES (@ID, @Version, @EventName, @Timestamp, @Payload);";
                    cmd.Parameters.AddWithValue("ID", evnt.Metadata.ID);
                    cmd.Parameters.AddWithValue("Version", evnt.Metadata.Version);
                    cmd.Parameters.AddWithValue("EventName", evnt.Metadata.EventName);
                    cmd.Parameters.AddWithValue("Timestamp", evnt.Metadata.Timestamp);
                    cmd.Parameters.AddWithValue("Payload", JsonConvert.SerializeObject(evnt.Payload));
                    cmd.ExecuteNonQuery();
                    long index = conn.ExecuteScalar<long>($@"SELECT Seq FROM {_streamName}_Events WHERE ID = @ID AND Version = @Version", new {
                        ID = evnt.Metadata.ID,
                        Version = evnt.Metadata.Version
                    });
                    return new EventMessage<TKey, object>(index, evnt);
                }
            }
        }
        
        public IEnumerable<EventMessage<TKey, object>> GetMissingEvents(long lastIndex)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString)) {
                conn.Open();
                return conn.Query<EventModel>($@"
                    SELECT * FROM {_streamName}_Events WHERE Seq > @LastIndex
                    ORDER BY Seq ASC", new { LastIndex = lastIndex }).Select(em =>
                        new EventMessage<TKey, object>(
                            em.Seq,
                            new DomainEvent<TKey, object>(
                                id: em.ID,
                                version: em.Version,
                                stream: _streamName,
                                eventName: em.EventName,
                                timestamp: em.Timestamp,
                                payload: _pharser(em.EventName, em.Payload)
                            )
                        )
                    );
            }
        }
    }
}