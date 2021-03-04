using System;

namespace Hermes.Worker.Core
{
    public class EventQueue
    {
        public string Name { get; }
        public string Exchange { get; }
        public Func<string, string, IEvent> ParseFn { get; }
        public long Index { get; set; }
        
        public EventQueue(string name, string exchange, Func<string, string, IEvent> parseFn)
        {
            Name = name;
            Exchange = exchange;
            ParseFn = parseFn;
            Index = 0;
        }
    };
}