namespace Contract
{
    public struct MappedInterval<TPayload>
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
    }
}