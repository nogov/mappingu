using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Contract;

namespace Console.Benchmarks
{
    public class SinglePutScenarios<TPayload> where TPayload : new()
    {
        private MappedInterval<TPayload>[] _input;

        [Params(Sorting.Ascending, Sorting.Descending, Sorting.Random)]
        public Sorting InputSorting { get; set; }

        [Params(Overlapping.No, Overlapping.Yes)]
        public Overlapping InputOverlapping { get; set; }

        [Params(100, 1000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var dummy = new TPayload();

            var start = 0L;
            var duration = 10L;
            var step = InputOverlapping == Overlapping.Yes ? duration >> 1 : duration << 1;

            _input = new MappedInterval<TPayload>[Count];
            switch (InputSorting)
            {
                case Sorting.Ascending:
                case Sorting.Descending:
                {
                    if (InputSorting == Sorting.Descending)
                    {
                        step *= -1;
                    }

                    for (var i = 0; i < Count; ++i)
                    {
                        _input[i] = new MappedInterval<TPayload>(start, start + duration, dummy);
                        start += step;
                    }

                    break;
                }
                case Sorting.Random:
                {
                    var durationFrom = (int)(duration - duration * 0.2);
                    var durationTo = (int)(duration + duration * 0.2);
                    var r = new Random(0xDEAD);
                    for (var i = 0; i < Count; ++i)
                    {
                        var s = r.Next(0, int.MaxValue);
                        var d = r.Next(durationFrom, durationTo);
                        _input[i] = new MappedInterval<TPayload>(s, d, dummy);   
                    }

                    break;
                }
            }
        }

        [Benchmark]
        [ArgumentsSource(nameof(CreateCollections))]
        public void Single_Ordered_NonOverlapping(IMappedIntervalsCollection<TPayload> collection)
        {
        }

        public static IEnumerable<object> CreateCollections()
        {
            return CollectionFactories.Factories.Select(f => f.Create<TPayload>());
        }
    }
}