using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Contract;

namespace Console.Benchmarks
{
    [InProcess] // It is now run in-process only, as separate executable won't load plugins and fail.
    public class SinglePutScenarios<TPayload> where TPayload : new()
    {
        private IMappedIntervalsCollection<TPayload> _collection;
        private MappedInterval<TPayload>[] _input;

        [Params(Sorting.Ascending)]
        //[ParamsAllValues]
        public Sorting InputSorting { get; set; }

        [Params(Overlapping.No)]
        //[ParamsAllValues]
        public Overlapping InputOverlapping { get; set; }

        [Params(10)]
        //[Params(100, 1000)]
        public int Count { get; set; }

        [ParamsSource(nameof(CreateCollections))]
        public CollectionDescription Description { get; set; }

        [IterationSetup]
        public void IterationSetup()
        {
            _collection = Description.Create<TPayload>();
        }

        [GlobalSetup]
        public void Setup()
        {
            _input = new MappedInterval<TPayload>[Count];
            DataGeneration.Fill(_input, InputSorting, InputOverlapping, new TPayload());
        }

        [Benchmark]
        public void Single_Ordered_NonOverlapping()
        {
            var collection = _collection;
            var box = new MappedInterval<TPayload>[1];
            for (var i = 0; i < _input.Length; ++i)
            {
                box[0] = _input[i];
                collection.Put(box);
            }
        }

        public static IEnumerable<object> CreateCollections()
        {
            return CollectionBag.Collections;
        }
    }
}