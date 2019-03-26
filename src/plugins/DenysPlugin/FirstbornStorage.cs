using System.Collections;
using System.Collections.Generic;
using Contract;

namespace DenysPlugin
{
    internal sealed class FirstbornStorage<T> : IMappedIntervalsCollection<T>
    {
        public int Count { get; }

        public void Put(IReadOnlyList<MappedInterval<T>> newIntervals)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(long from, long to)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<MappedInterval<T>> GetEnumerator(long from)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<MappedInterval<T>> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}