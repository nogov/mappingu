using Contract;

namespace DenysPlugin
{
    public sealed class FirstbornStorageFactory : SandboxPlugin
    {
        public FirstbornStorageFactory(ILogger logger)
            : base(logger)
        {
        }

        public override string Name => "Firstborn";

        public override IMappedIntervalsCollection<T> CreateCollection<T>()
        {
            return new FirstbornStorage<T>();
        }
    }
}