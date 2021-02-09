using System;

namespace Hermes.Worker.Core.Ports
{
    public interface IUnitOfWorkPort<IO>
    {
        void Execute(Action<IO> unitOfWork);
        void Transaction(Action<IO> unitOfWork);
    }
    public interface DBInterpreter<IO> {}
}