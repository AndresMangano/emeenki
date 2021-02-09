namespace Hermes.Worker.Core.Repositories.Helpers
{
    public class DbUpdate<TValue>
    {
        public TValue Value { get; }
        public DbUpdate(TValue value) {
            Value = value;
        }
    }
}