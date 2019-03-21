using BenchmarkDotNet.Attributes;
using Contract;

namespace Console.Benchmarks
{
    [InProcess] // It is now run in-process only, as separate executable won't load plugins and fail.
    public class DeleteScenarios<TPayload> : CollectionBenchmarkBase<TPayload>
        where TPayload : new()
    {
        private MappedInterval<TPayload>[] _toBeDeleted;

        [Params(PreDeleteState.OneBigInterval)]
        //[ParamsAllValues]
        public PreDeleteState InitialState { get; set; }

        [Params(Sorting.Random)]
        //[ParamsAllValues]
        public Sorting DeletionPattern { get; set; }

        [Params(10)]
        //[Params(100, 1000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var dummy = new TPayload();
            _toBeDeleted = new MappedInterval<TPayload>[Count];
            DataGeneration.Fill(_toBeDeleted, DeletionPattern, Overlapping.Yes, dummy);
        }

        protected override void AfterCollectionCreation()
        {
            var dummy = new TPayload();
            var box = new MappedInterval<TPayload>[1];
            switch (InitialState)
            {
                case PreDeleteState.OneBigInterval:
                    box[0] = new MappedInterval<TPayload>(0, long.MaxValue, dummy);
                    Collection.Put(box);
                    break;
                case PreDeleteState.LotsOfSmallIntervals:
                case PreDeleteState.Random:
                    var inputs = new MappedInterval<TPayload>[Count];
                    var sorting = InitialState == PreDeleteState.Random ? Sorting.Random : Sorting.Ascending;
                    DataGeneration.Fill(inputs, sorting, Overlapping.No, dummy);
                    Collection.Put(inputs);
                    break;
            }

            base.AfterCollectionCreation();
        }

        [Benchmark]
        public void Work()
        {
            var collection = Collection;
            for (var i = 0; i < _toBeDeleted.Length; ++i)
            {
                collection.Delete(_toBeDeleted[i].IntervalStart, _toBeDeleted[i].IntervalEnd);
            }
        }
    }
}