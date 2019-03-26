using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Contract;

namespace DenysPlugin
{
    internal sealed class StraightforwardStorage<T> : IMappedIntervalsCollection<T>
    {
        private List<MappedInterval<T>> _current = new List<MappedInterval<T>>();
        private List<MappedInterval<T>> _old = new List<MappedInterval<T>>();

        public int Count => _current.Count;

        public void Put(IReadOnlyList<MappedInterval<T>> newIntervals)
        {
            PrepareNextGeneration();
            _current.AddRange(Rebuild(_old, newIntervals));
        }

        public void Delete(long from, long to)
        {
            PrepareNextGeneration();
            _current.AddRange(Slice(_old, from, to));
        }

        public IEnumerator<MappedInterval<T>> GetEnumerator(long from)
        {
            return _current.Where(i => i.IntervalEnd >= from).GetEnumerator();
        }

        public IEnumerator<MappedInterval<T>> GetEnumerator()
        {
            return _current.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void PrepareNextGeneration()
        {
            var temp = _old;
            _old = _current;
            _current = temp;
            _current.Clear();
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

        private static IEnumerable<MappedInterval<T>> GhettoMergeSort(IEnumerable<MappedInterval<T>> current, IEnumerable<MappedInterval<T>> addition)
        {
            using (var existingEnumerator = current.GetEnumerator())
            using (var addedEnumerator = addition.GetEnumerator())
            {
                IEnumerator<MappedInterval<T>> tail;

                MappedInterval<T>? existing = null;
                MappedInterval<T>? added = null;

                while (true)
                {
                    if (!existing.HasValue)
                    {
                        if (!existingEnumerator.MoveNext())
                        {
                            tail = addedEnumerator;
                            break;
                        }
                        existing = existingEnumerator.Current;
                    }

                    while (addedEnumerator.MoveNext())
                    {
                        added = addedEnumerator.Current;

                        if (added.Value.IntervalStart < existing.Value.IntervalStart)
                        {
                            yield return added.Value;
                            added = null;
                            continue;
                        }
                        break;
                    }

                    yield return existing.Value;
                    existing = null;

                    if (!added.HasValue)
                    {
                        tail = existingEnumerator;
                        break;
                    }

                    while (existingEnumerator.MoveNext())
                    {
                        existing = existingEnumerator.Current;
                        if (existing.Value.IntervalStart < added.Value.IntervalStart)
                        {
                            yield return existing.Value;
                            existing = null;
                            continue;
                        }
                        break;
                    }

                    yield return added.Value;
                    added = null;
                }

                while (tail.MoveNext())
                {
                    yield return tail.Current;
                }
            }
        }

        private static IEnumerable<MappedInterval<T>> Rebuild(IEnumerable<MappedInterval<T>> current, IEnumerable<MappedInterval<T>> addition)
        {
            var set = GhettoMergeSort(current, addition);

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