using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Contract;

namespace DummyPlugin
{
    internal sealed class MappedIntervalsCollection<T> : IMappedIntervalsCollection<T>
    {
        private readonly SortedSet<Hueta> _sortedSet;

        public MappedIntervalsCollection()
        {
            _sortedSet = new SortedSet<Hueta>(new HuetaComparer());
        }

        public IEnumerator<MappedInterval<T>> GetEnumerator()
        {
            return _sortedSet.Select(hueta => hueta.Interval).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _sortedSet.Count; }
        }

        public void Put(IEnumerable<MappedInterval<T>> newIntervals)
        {
            foreach (var mappedInterval in newIntervals)
            {
                Delete(mappedInterval.IntervalStart, mappedInterval.IntervalEnd);
                _sortedSet.Add(new Hueta(mappedInterval, null));
            }
        }

        public void Delete(long from, long to)
        {
            var ololol = _sortedSet.GetViewBetween(new Hueta(new MappedInterval<T>(from, from, default(T)), true), new Hueta(_sortedSet.Max.Interval, false));
            if (ololol.Min.Start < from)
            {
                _sortedSet.Remove(ololol.Min);
                _sortedSet.Add(new Hueta(new MappedInterval<T>(ololol.Min.Start, from, ololol.Min.Interval.Payload), null));
                ololol.Remove(ololol.Min);
            }
            if (ololol.Max.End > to)
            {
                _sortedSet.Remove(ololol.Max);
                _sortedSet.Add(new Hueta(new MappedInterval<T>(to, ololol.Max.End, ololol.Max.Interval.Payload), null));
                ololol.Remove(ololol.Max);
            }

            _sortedSet.ExceptWith(ololol);
        }

        public IEnumerator<MappedInterval<T>> GetEnumerator(long from)
        {
            return _sortedSet.GetViewBetween(new Hueta(new MappedInterval<T>(from, from, default(T)), true), new Hueta(_sortedSet.Max.Interval, false)).Select(hueta => hueta.Interval).GetEnumerator();
        }

        private struct Hueta
        {
            public Hueta(MappedInterval<T> interval, bool? isStart)
            {
                Interval = interval;
                if (isStart.HasValue)
                {
                    IsStart = isStart.Value;
                }
                else
                {
                    IsStart = null;
                }
            }

            public MappedInterval<T> Interval { get; }

            public long Start => Interval.IntervalStart;

            public long End => Interval.IntervalEnd;

            public bool? IsStart { get; }
        }

        class HuetaComparer : IComparer<Hueta>
        {
            public int Compare(Hueta interval1, Hueta interval2)
            {
                if (IsCompletelyWithin(interval1, interval2))
                {
                    return 0;
                }

                if (Overlaps(interval1, interval2))
                {
                    if (interval1.IsStart.HasValue && interval1.IsStart.Value)
                    {
                        return -1;
                    }

                    if (interval2.IsStart.HasValue && interval2.IsStart.Value)
                    {
                        return 1;
                    }
                    
                    if (interval1.IsStart.HasValue && !interval1.IsStart.Value)
                    {
                        return 1;
                    }

                    if (interval2.IsStart.HasValue && !interval2.IsStart.Value)
                    {
                        return -1;
                    }
                }

                return (int)(interval1.Start - interval2.Start);
            }

            private static bool IsCompletelyWithin(Hueta interval1, Hueta interval2)
            {
                return interval1.Start >= interval2.Start && interval1.End <= interval2.End;
            }

            private static bool Overlaps(Hueta interval1, Hueta interval2)
            {
                return interval1.Start <= interval2.End && interval1.End >= interval2.Start;
            }
        }
    }
}
