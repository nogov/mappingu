using System.Collections.Generic;
using Contract;

namespace Console.Benchmarks
{
    public sealed class CollectionDescription
    {
        private readonly SandboxPlugin _plugin;

        public CollectionDescription(SandboxPlugin plugin)
        {
            _plugin = plugin;
        }

        public IMappedIntervalsCollection<T> Create<T>()
        {
            return _plugin.CreateCollection<T>();
        }

        public override string ToString()
        {
            return _plugin.Name;
        }
    }

    internal static class CollectionBag
    {
        private static readonly List<CollectionDescription> Storage = new List<CollectionDescription>();

        public static IEnumerable<CollectionDescription> Collections => Storage;

        public static void Register(CollectionDescription c)
        {
            Storage.Add(c);
        }

        public static void Cleanup()
        {
            Storage.Clear();
        }
    }
}