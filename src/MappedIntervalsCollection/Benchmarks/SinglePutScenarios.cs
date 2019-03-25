using BenchmarkDotNet.Attributes;
using Contract;

namespace Console.Benchmarks
{
    [InProcess] // It is now run in-process only, as separate executable won't load plugins and fail.
    public class SinglePutScenarios<TPayload> : CollectionBenchmarkBase<TPayload>
        where TPayload : new()
    {
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

        [GlobalSetup]
        public void Setup()
        {
            _input = new MappedInterval<TPayload>[Count];
            DataGeneration.Fill(InputSorting, InputOverlapping, new TPayload(), _input);
        }

        [Benchmark]
        public void Work()
        {
            var collection = Collection;
            var box = new MappedInterval<TPayload>[1];
            for (var i = 0; i < _input.Length; ++i)
            {
                box[0] = _input[i];
                collection.Put(box);
            }
        }
    }
}