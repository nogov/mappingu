using System;
using Contract;

namespace Console.Benchmarks
{
    internal sealed class DataGeneration
    {
        public static Tuple<long, long> Fill<TPayload>(MappedInterval<TPayload>[] input, Sorting sorting, Overlapping overlapping, TPayload filler)
        {
            var count = input.Length;
            var start = 0L;
            var duration = 10L;
            var step = overlapping == Overlapping.Yes ? duration >> 1 : duration << 1;

            var min = long.MaxValue;
            var max = long.MinValue;

            switch (sorting)
            {
                case Sorting.Ascending:
                case Sorting.Descending:
                {
                    var from = sorting == Sorting.Ascending ? 0 : count - 1;
                    var to = sorting == Sorting.Ascending ? count : 0;
                    var delta = sorting == Sorting.Ascending ? 1 : -1;

                    while (from != to)
                    {
                        min = Math.Min(min, start);
                        max = Math.Max(max, start + duration);

                        input[from] = new MappedInterval<TPayload>(start, start + duration, filler);
                        start += step;
                        from += delta;
                    }

                    break;
                }
                case Sorting.Random:
                {
                    var durationFrom = (int)(duration - duration * 0.2);
                    var durationTo = (int)(duration + duration * 0.2);
                    var r = new Random(0xDEAD);
                    for (var i = 0; i < count; ++i)
                    {
                        var s = r.Next(0, int.MaxValue);
                        var d = r.Next(durationFrom, durationTo);
                        min = Math.Min(min, s);
                        max = Math.Max(max, s + d);
                        input[i] = new MappedInterval<TPayload>(s, s + d, filler);   
                    }

                    break;
                }
            }

            return Tuple.Create(min, max);
        }
    }
}