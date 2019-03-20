using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public interface IMappedIntervalsCollection<TPayload> : IReadOnlyCollection<MappedInterval<TPayload>>
    {
        void Put(IEnumerable<MappedInterval<TPayload>> newIntervals);

        void Delete(long from, long to);

        IEnumerator<MappedInterval<TPayload>> GetEnumerator(long from);
    }
}
