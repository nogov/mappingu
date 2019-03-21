using BenchmarkDotNet.Attributes;
using Contract;

namespace Console.Benchmarks
{
    [InProcess] // It is now run in-process only, as separate executable won't load plugins and fail.
    public class SingleDeleteScenarios<TPayload> : CollectionBenchmarkBase<TPayload>
        where TPayload : new()
    {
        private MappedInterval<TPayload>[] _input;
        private long _min, _max;

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
            var mm = DataGeneration.Fill(_input, InputSorting, InputOverlapping, new TPayload());
            _min = mm.Item1 - Nudge(mm.Item1);
            _max = mm.Item2 + Nudge(mm.Item2);
        }

        [Benchmark]
        public void Work()
        {
            Collection.Delete(_min, _max);
        }

        private static long Nudge(long value)
        {
            return (long)(value * 0.05);
        }
    }
}