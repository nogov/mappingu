namespace Contract
{
    public abstract class SandboxPlugin
    {
        public SandboxPlugin()
        {
        }

        public abstract string Name { get; }

        public abstract IMappedIntervalsCollection<T> CreateCollection<T>();
    }
}