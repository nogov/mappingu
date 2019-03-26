using Contract;

namespace DenysPlugin
{
    public sealed class StraightforwardStorageFactory : SandboxPlugin
    {
        public StraightforwardStorageFactory(ILogger logger)
            : base(logger)
        {
        }

        public override string Name => "StraightforwardStorage";

        public override IMappedIntervalsCollection<T> CreateCollection<T>()
        {
            return new StraightforwardStorage<T>();
        }
    }
}
