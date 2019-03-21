using System;
using System.Collections.Generic;

namespace Console.Benchmarks
{
    public struct ValueCrate<T> : IEquatable<ValueCrate<T>>
    {
        public ValueCrate(T value)
        {
            Value = value;
        }

        public T Value { get; }

        public bool Equals(ValueCrate<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ValueCrate<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Value);
        }

        public static bool operator ==(ValueCrate<T> left, ValueCrate<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ValueCrate<T> left, ValueCrate<T> right)
        {
            return !left.Equals(right);
        }
    }
}