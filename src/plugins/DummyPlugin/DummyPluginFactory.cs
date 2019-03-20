using Contract;

namespace DummyPlugin
{
    public sealed class DummyPluginFactory : SandboxPlugin
    {
        public override string Name => nameof(DummyPluginFactory);

        public override IMappedIntervalsCollection<T> CreateCollection<T>()
        {
            return new MappedIntervalsCollection<T>();
        }
    }
}