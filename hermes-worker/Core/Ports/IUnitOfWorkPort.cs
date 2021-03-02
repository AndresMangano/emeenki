using System;
using Hermes.Worker.Core.Model;

namespace Hermes.Worker.Core.Ports
{
    public interface IUnitOfWorkPort<IO>
    {
        void Transaction<K, V>(DefaultMessage<K, V> message, Action<IO> unitOfWork);
    }
    public interface DBInterpreter<IO> {}
}