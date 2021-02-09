using System;

namespace Hermes.Worker.Core.Ports
{
    public interface IRabbitMQPort
    {
        void CreateModelAndWait(Action<IRabbitMQHandler> model);
    }
    
    public delegate void DefaultConsumer(string routingKey, string message);
    public interface IRabbitMQHandler
    {
        void DeclareRoute(string queue, string exchange, DefaultConsumer consumer);
    }
}