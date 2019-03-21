using System;
using System.Collections;
using System.Collections.Generic;
using Contract;

namespace DenysPlugin
{
    internal sealed class Storage<T> : IMappedIntervalsCollection<T>
    {
        public int Count { get; }

        public void Put(IReadOnlyList<MappedInterval<T>> newIntervals)
        {
            throw new NotImplementedException();
        }

        public void Delete(long from, long to)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<MappedInterval<T>> GetEnumerator(long @from)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<MappedInterval<T>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}