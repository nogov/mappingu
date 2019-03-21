using System;
using System.Collections.Generic;
using Contract;

namespace Tests
{
    internal static class CollectionFactories
    {
        private static readonly List<Func<IMappedIntervalsCollection<Crate>>> Storage = new List<Func<IMappedIntervalsCollection<Crate>>>();

        public static void RegisterFactory(Func<IMappedIntervalsCollection<Crate>> factory)
        {
            Storage.Add(factory);
        }

        public static void Cleanup()
        {
            Storage.Clear();
        }

        public static IEnumerable<Func<IMappedIntervalsCollection<Crate>>> Factories => Storage;
    }
}