using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Contract;
using NUnit.Framework;

namespace Tests
{
    [TestFixtureSource(typeof(CollectionFactories), nameof(CollectionFactories.Factories))]
    internal sealed class MappedIntervalsCollectionRealLikeDataFixture
    {
        private readonly Func<IMappedIntervalsCollection<Crate>> _factory;

        private IMappedIntervalsCollection<Crate> _sut;

        public MappedIntervalsCollectionRealLikeDataFixture(Func<IMappedIntervalsCollection<Crate>> factory)
        {
            _factory = factory;
        }

        [SetUp]
        public void SetUp()
        {
            _sut = _factory();
        }

        [TestCase(DataSource.Glasses2, DataFilter.Raw)]
        [TestCase(DataSource.Glasses2, DataFilter.IvtFixation)]
        [TestCase(DataSource.Glasses2, DataFilter.IvtAttention)]
        [TestCase(DataSource.Spectrum, DataFilter.Raw)]
        [TestCase(DataSource.Spectrum, DataFilter.IvtFixation)]
        [TestCase(DataSource.Spectrum, DataFilter.IvtAttention)]
        public void Ascending(DataSource source, DataFilter filter)
        {
            var input = new MappedInterval<Crate>[10];
            DataGeneration.LikeReal(source, filter, MakeCrate, input);

            var whole = new Tuple<int, int>[1];
            whole[0] = Tuple.Create(0, input.Length);
            AddSeries(input, whole);

            CollectionAssert.AreEqual(_sut, input);
        }

        [TestCase(DataSource.Glasses2, DataFilter.Raw)]
        [TestCase(DataSource.Glasses2, DataFilter.IvtFixation)]
        [TestCase(DataSource.Glasses2, DataFilter.IvtAttention)]
        [TestCase(DataSource.Spectrum, DataFilter.Raw)]
        [TestCase(DataSource.Spectrum, DataFilter.IvtFixation)]
        [TestCase(DataSource.Spectrum, DataFilter.IvtAttention)]
        public void Descending(DataSource source, DataFilter filter)
        {
            var pre = new MappedInterval<Crate>[10];
            var input = new MappedInterval<Crate>[pre.Length];
            DataGeneration.LikeReal(source, filter, MakeCrate, pre);
            Reverse(pre, input);

            var whole = new Tuple<int, int>[1];
            whole[0] = Tuple.Create(0, input.Length);
            AddSeries(input, whole);

            CollectionAssert.AreEqual(_sut, pre);
        }

        [TestCase(DataSource.Glasses2, DataFilter.Raw)]
        [TestCase(DataSource.Glasses2, DataFilter.IvtFixation)]
        [TestCase(DataSource.Glasses2, DataFilter.IvtAttention)]
        [TestCase(DataSource.Spectrum, DataFilter.Raw)]
        [TestCase(DataSource.Spectrum, DataFilter.IvtFixation)]
        [TestCase(DataSource.Spectrum, DataFilter.IvtAttention)]
        public void Random(DataSource source, DataFilter filter)
        {
            var pre = new MappedInterval<Crate>[10];
            var input = new MappedInterval<Crate>[pre.Length];
            DataGeneration.LikeReal(source, filter, MakeCrate, pre);
            Shuffle(pre, input);

            var whole = new Tuple<int, int>[1];
            whole[0] = Tuple.Create(0, input.Length);
            AddSeries(input, whole);

            CollectionAssert.AreEqual(_sut, pre);
        }

        [TestCase(DataSource.Glasses2, DataFilter.Raw)]
        [TestCase(DataSource.Glasses2, DataFilter.IvtFixation)]
        [TestCase(DataSource.Glasses2, DataFilter.IvtAttention)]
        [TestCase(DataSource.Spectrum, DataFilter.Raw)]
        [TestCase(DataSource.Spectrum, DataFilter.IvtFixation)]
        [TestCase(DataSource.Spectrum, DataFilter.IvtAttention)]
        public void RandomAscendingSeries(DataSource source, DataFilter filter)
        {
            var input = new MappedInterval<Crate>[10];
            DataGeneration.LikeReal(source, filter, MakeCrate, input);

            var preRanges = GeneratePartitions(input.Length).ToArray();
            var ranges = new Tuple<int, int>[preRanges.Length];
            Shuffle(preRanges, ranges);

            AddSeries(input, ranges);

            CollectionAssert.AreEqual(_sut, input);
        }

        [TestCase(DataSource.Glasses2, DataFilter.Raw)]
        [TestCase(DataSource.Glasses2, DataFilter.IvtFixation)]
        [TestCase(DataSource.Glasses2, DataFilter.IvtAttention)]
        [TestCase(DataSource.Spectrum, DataFilter.Raw)]
        [TestCase(DataSource.Spectrum, DataFilter.IvtFixation)]
        [TestCase(DataSource.Spectrum, DataFilter.IvtAttention)]
        public void RandomDescendingSeries(DataSource source, DataFilter filter)
        {
            var pre = new MappedInterval<Crate>[10];
            var input = new MappedInterval<Crate>[pre.Length];
            DataGeneration.LikeReal(source, filter, MakeCrate, pre);
            Reverse(pre, input);

            var preRanges = GeneratePartitions(input.Length).ToArray();
            var ranges = new Tuple<int, int>[preRanges.Length];
            Shuffle(preRanges, ranges);

            AddSeries(input, ranges);

            CollectionAssert.AreEqual(_sut, pre);
        }

        [TestCase(DataSource.Glasses2, DataFilter.Raw)]
        [TestCase(DataSource.Glasses2, DataFilter.IvtFixation)]
        [TestCase(DataSource.Glasses2, DataFilter.IvtAttention)]
        [TestCase(DataSource.Spectrum, DataFilter.Raw)]
        [TestCase(DataSource.Spectrum, DataFilter.IvtFixation)]
        [TestCase(DataSource.Spectrum, DataFilter.IvtAttention)]
        public void RandomSeriesBatched(DataSource source, DataFilter filter)
        {
            var input = new MappedInterval<Crate>[10];
            DataGeneration.LikeReal(source, filter, MakeCrate, input);

            var preRanges = GeneratePartitions(input.Length).ToArray();
            var ranges = new Tuple<int, int>[preRanges.Length];
            Shuffle(preRanges, ranges);

            foreach (var range in ranges)
            {
                _sut.Put(new ArraySegment<MappedInterval<Crate>>(input, range.Item1, range.Item2 - range.Item1));
            }

            CollectionAssert.AreEqual(_sut, input);
        }

        private void AddSeries(IReadOnlyList<MappedInterval<Crate>> input, IReadOnlyList<Tuple<int, int>> ranges)
        {
            var box = new MappedInterval<Crate>[1];

            foreach (var range in ranges)
            {
                for (var i = range.Item1; i < range.Item2; ++i)
                {
                    box[0] = input[i];
                    _sut.Put(box);
                }
            }
        }

        private static Crate MakeCrate(int x)
        {
            return new Crate(x);
        }

        private static void Shuffle<T>(IReadOnlyList<T> input, T[] output)
        {
            Debug.Assert(input.Count == output.Length, "Must be matching.");
            var rand = new Random(0xC0FFEE);
            for (var i = 0; i < input.Count; ++i)
            {
                var j = rand.Next(0, i + 1);
                output[i] = output[j];
                output[j] = input[i];
            }
        }

        private static void Reverse<T>(IReadOnlyList<T> input, T[] output)
        {
            Debug.Assert(input.Count == output.Length, "Must be matching.");
            for (var i = 0; i < input.Count; ++i)
            {
                output[i] = input[input.Count - 1 - i];
            }
        }

        private static IEnumerable<Tuple<int, int>> GeneratePartitions(int count)
        {
            var r = new Random(0xDADB0B);
            var average = count < 16 ? count / 2 : count / 16;
            var current = 0;
            while (current < count)
            {
                var from = current;
                current = Math.Min(current + r.Next(average), count);
                yield return Tuple.Create(from, current);
            }
        }

        public enum DataSource
        {
            Glasses2 = 0,
            Spectrum,
        }

        public enum DataFilter
        {
            Raw = 0,
            IvtFixation,
            IvtAttention,
        }

        public sealed class DataGeneration
        {
            private static readonly Dictionary<Tuple<DataSource, DataFilter>, Description> RealLikeGenerators = new Dictionary<Tuple<DataSource, DataFilter>, Description>
        {
            { Tuple.Create(DataSource.Glasses2, DataFilter.Raw),          new Description( 10,  10, 19, 20) },
            { Tuple.Create(DataSource.Glasses2, DataFilter.IvtAttention), new Description(100, 700, 18, 25) },
            { Tuple.Create(DataSource.Glasses2, DataFilter.IvtFixation),  new Description(120, 500, 40, 50) },

            { Tuple.Create(DataSource.Spectrum, DataFilter.Raw),          new Description(  2,    3,  2,  3) },
            { Tuple.Create(DataSource.Spectrum, DataFilter.IvtAttention), new Description(120, 1600, 10, 75) },
            { Tuple.Create(DataSource.Spectrum, DataFilter.IvtFixation),  new Description(120,  600, 15, 80) },
        };

            public static Tuple<long, long> LikeReal<TPayload>(DataSource dataSource, DataFilter dataFilter, Func<int, TPayload> makePayload, MappedInterval<TPayload>[] output)
            {
                if (RealLikeGenerators.TryGetValue(Tuple.Create(dataSource, dataFilter), out var description))
                {
                    MakeSpacedIntervals(description, makePayload, output);
                    return Tuple.Create(output[0].IntervalStart, output[output.Length - 1].IntervalEnd);
                }
                throw new ArgumentException(FormattableString.Invariant($"Can't find desired data generator for {dataSource}/{dataFilter}."), nameof(dataSource) + "/" + nameof(dataFilter));
            }

            private static void MakeSpacedIntervals<TPayload>(Description description, Func<int, TPayload> makePayload, MappedInterval<TPayload>[] output)
            {
                var r = new Random(0xBADDAD);

                var start = MakeGap();
                for (var i = 0; i < output.Length; ++i)
                {
                    start += MakeGap();
                    var duration = MakeInterval();
                    var from = ToUsec(start);
                    var to = from + duration;
                    output[i] = new MappedInterval<TPayload>(from, to, makePayload(i));
                }

                int MakeInterval() => r.Next(description.IntervalRange.Item1, description.IntervalRange.Item2);
                int MakeGap() => r.Next(description.GapRange.Item1, description.GapRange.Item2);
                long ToUsec(int msec) => msec * 1000 - r.Next(0, 3);
            }

            private sealed class Description
            {
                public Description(int intervalMin, int intervalMax, int gapMin, int gapMax)
                {
                    IntervalRange = Tuple.Create(intervalMin, intervalMax);
                    GapRange = Tuple.Create(gapMin, gapMax);
                }

                public Tuple<int, int> IntervalRange { get; }

                public Tuple<int, int> GapRange { get; }
            }
        }
    }
}