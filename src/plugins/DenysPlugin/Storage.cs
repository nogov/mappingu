using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Contract;

namespace DenysPlugin
{
    internal sealed class Storage<T> : IMappedIntervalsCollection<T>
    {
        private IReadOnlyList<MappedInterval<T>> _intervals = new MappedInterval<T>[0];

        public int Count => _intervals.Count;

        public void Put(IReadOnlyList<MappedInterval<T>> newIntervals)
        {
            _intervals = Rebuild(_intervals, newIntervals).ToArray();
        }

        public void Delete(long from, long to)
        {
            _intervals = Slice(_intervals, from, to).ToArray();
        }

        public IEnumerator<MappedInterval<T>> GetEnumerator(long from)
        {
            return _intervals.Where(i => i.IntervalEnd >= from).GetEnumerator();
        }

        public IEnumerator<MappedInterval<T>> GetEnumerator()
        {
            return _intervals.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static IEnumerable<MappedInterval<T>> Slice(IReadOnlyList<MappedInterval<T>> current, long from, long to)
        {
            foreach (var i in current)
            {
                if (i.IntervalStart < from && i.IntervalEnd <= to)
                {
                    yield return i;
                    continue;
                }

                if (i.IntervalStart >= from && i.IntervalEnd <= to)
                {
                    continue;
                }

                if (from >= i.IntervalStart && to <= i.IntervalEnd)
                {
                    if (i.IntervalStart != from)
                    {
                        yield return new MappedInterval<T>(i.IntervalStart, from, i.Payload);
                    }

                    if (i.IntervalEnd != to)
                    {
                        yield return new MappedInterval<T>(to, i.IntervalEnd, i.Payload);
                    }

                    continue;
                }

                yield return i;
            }
        }

        private static IEnumerable<MappedInterval<T>> Rebuild(IReadOnlyList<MappedInterval<T>> current, IReadOnlyList<MappedInterval<T>> addition)
        {
            var set = current.Concat(addition).OrderBy(i => i.IntervalStart);

            MappedInterval<T>? pending = null;

            foreach (var i in set)
            {
                if (!pending.HasValue)
                {
                    pending = i;
                    continue;
                }

                if (i.Payload.Equals(pending.Value.Payload))
                {
                    if (pending.Value.IntervalEnd >= i.IntervalStart)
                    {
                        pending = new MappedInterval<T>(pending.Value.IntervalStart, i.IntervalEnd, i.Payload);
                        continue;
                    }

                    yield return pending.Value;
                    pending = i;
                    continue;
                }

                if (pending.Value.IntervalEnd >= i.IntervalStart)
                {
                    yield return new MappedInterval<T>(pending.Value.IntervalStart, i.IntervalStart, pending.Value.Payload);
                }
                else
                {
                    yield return pending.Value;
                }

                pending = i;
            }

            if (pending.HasValue)
            {
                yield return pending.Value;
            }
        }
    }
}