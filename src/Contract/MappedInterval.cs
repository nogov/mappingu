using System;
using System.Collections.Generic;

namespace Contract
{
    public struct MappedInterval<TPayload> : IEquatable<MappedInterval<TPayload>>
    {
        public MappedInterval(long intervalStart, long intervalEnd, TPayload payload)
        {
            IntervalStart = intervalStart;
            IntervalEnd = intervalEnd;
            Payload = payload;
        }

        public long IntervalStart { get; }

        public long IntervalEnd { get; }

        public TPayload Payload { get; }

        public bool Equals(MappedInterval<TPayload> other)
        {
            return IntervalStart == other.IntervalStart && IntervalEnd == other.IntervalEnd && EqualityComparer<TPayload>.Default.Equals(Payload, other.Payload);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is MappedInterval<TPayload> other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IntervalStart.GetHashCode();
                hashCode = (hashCode * 397) ^ IntervalEnd.GetHashCode();
                hashCode = (hashCode * 397) ^ EqualityComparer<TPayload>.Default.GetHashCode(Payload);
                return hashCode;
            }
        }

        public static bool operator ==(MappedInterval<TPayload> left, MappedInterval<TPayload> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MappedInterval<TPayload> left, MappedInterval<TPayload> right)
        {
            return !left.Equals(right);
        }
    }
}