using System;
using Contract;

namespace Console.Benchmarks
{
    internal sealed class DataGeneration
    {
        public static void Fill<TPayload>(MappedInterval<TPayload>[] input, Sorting sorting, Overlapping overlapping, TPayload filler)
        {
            var count = input.Length;
            var start = 0L;
            var duration = 10L;
            var step = overlapping == Overlapping.Yes ? duration >> 1 : duration << 1;

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
                        input[i] = new MappedInterval<TPayload>(s, d, filler);   
                    }

                    break;
                }
            }
        }
    }
}