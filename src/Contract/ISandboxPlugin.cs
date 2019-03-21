namespace Contract
{
    public abstract class SandboxPlugin
    {
        protected SandboxPlugin(ILogger logger)
        {
            Logger = logger;
        }

        protected ILogger Logger { get; }

        public abstract string Name { get; }

        public abstract IMappedIntervalsCollection<T> CreateCollection<T>();
    }
}