namespace Console.Benchmarks
{
    public sealed class ReferenceCrate<T>
    {
        public ReferenceCrate()
        {
            Value = default(T);
        }

        public ReferenceCrate(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }
}