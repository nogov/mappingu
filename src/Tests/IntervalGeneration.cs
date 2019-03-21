using System;
using System.Collections.Generic;
using System.Diagnostics;
using Contract;

namespace Tests
{
    public static class IntervalGeneration
    {
        public static IEnumerable<MappedInterval<T>> DashedSequence<T>(long from, long step, int count, Func<int, T> makePayload)
        {
            Debug.Assert(step > 1);
            return Sequence(from, step >> 1, step, count, makePayload);
        }

        public static IEnumerable<MappedInterval<T>> Sequence<T>(long from, long duration, long step, int count, Func<int, T> makePayload)
        {
            var time = from;
            for (var i = 0; i < count; ++i)
            {
                yield return new MappedInterval<T>(time, time + duration, makePayload(i));
                time += step;
            }
        }
    }
}