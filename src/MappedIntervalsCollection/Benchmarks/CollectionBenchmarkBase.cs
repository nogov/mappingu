﻿using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Contract;

namespace Console.Benchmarks
{
    public abstract class CollectionBenchmarkBase<TPayload>
        where TPayload : new()
    {
        protected IMappedIntervalsCollection<TPayload> Collection;

        [ParamsSource(nameof(CreateCollections))]
        public CollectionDescription Description { get; set; }

        [IterationSetup]
        public void IterationSetup()
        {
            Collection = Description.Create<TPayload>();
            AfterCollectionCreation();
        }

        public static IEnumerable<object> CreateCollections()
        {
#if BENCHMARKING_OUTSIDE
            return MefDlyaBednyx.GetPlugins(new ConsoleLogger()).Select(p => new CollectionDescription(p));
#else
            return CollectionBag.Collections;
#endif
        }

        protected virtual void AfterCollectionCreation()
        {
        }
    }
}