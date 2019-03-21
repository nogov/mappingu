namespace Contract
{
    public abstract class SandboxPlugin
    {
        protected SandboxPlugin()
        {
        }

        public abstract string Name { get; }

        public abstract IMappedIntervalsCollection<T> CreateCollection<T>();
    }
}