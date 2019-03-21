using Contract;

namespace DummyPlugin
{
    public sealed class DummyPluginFactory : SandboxPlugin
    {
        public DummyPluginFactory(ILogger logger)
            : base(logger)
        {
        }

        public override string Name => nameof(DummyPluginFactory);

        public override IMappedIntervalsCollection<T> CreateCollection<T>()
        {
            return new MappedIntervalsCollection<T>();
        }
    }
}