using System.Collections.Generic;

namespace Contract
{
    public interface IMappedIntervalsCollection<TPayload> : IReadOnlyCollection<MappedInterval<TPayload>>
    {
        void Put(IReadOnlyList<MappedInterval<TPayload>> newIntervals);

        void Delete(long from, long to);

        IEnumerator<MappedInterval<TPayload>> GetEnumerator(long from);
    }
}
