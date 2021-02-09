using System;

namespace Hermes.Shell
{
    public sealed class InfrastructureException : Exception
    {
        public InfrastructureException(string message) : base(message){}
    }
}