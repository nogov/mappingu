using System.Collections.Generic;
using Contract;

namespace Console.Benchmarks
{
    internal sealed class CollectionFactory
    {
        private readonly SandboxPlugin _plugin;

        public CollectionFactory(SandboxPlugin plugin)
        {
            _plugin = plugin;
        }

        public IMappedIntervalsCollection<T> Create<T>()
        {
            return _plugin.CreateCollection<T>();
        }
    }

    internal static class CollectionFactories
    {
        private static readonly List<CollectionFactory> Storage = new List<CollectionFactory>();

        public static void RegisterFactory(CollectionFactory factory)
        {
            Storage.Add(factory);
        }

        public static void Cleanup()
        {
            Storage.Clear();
        }

        public static IEnumerable<CollectionFactory> Factories => Storage;
    }
}