using System;
using System.Collections.Generic;
using System.Linq;
using Contract;
using NUnit.Framework;

namespace Tests
{
    [TestFixtureSource(typeof(CollectionFactories), nameof(CollectionFactories.Factories))]
    internal sealed class MappedIntervalsCollectionFixture
    {
        private readonly Func<IMappedIntervalsCollection<Crate>> _factory;

        private IMappedIntervalsCollection<Crate> _sut;

        public MappedIntervalsCollectionFixture(Func<IMappedIntervalsCollection<Crate>> factory)
        {
            _factory = factory;
        }

        [SetUp]
        public void SetUp()
        {
            _sut = _factory();
        }

        [Test]
        public void NonOverwritingUpdate()
        {
            var input = IntervalGeneration.DashedSequence(0, 2, 10, MakeDummy).ToArray();

            _sut.Put(input);

            var output = _sut.ToArray();
            CollectionAssert.AreEqual(input, output);
        }

        [Test]
        public void OverwritingUpdate()
        {
            var input = IntervalGeneration.Sequence(0, 10, 5, 5, MakeDummy).ToArray();

            var space = new MappedInterval<Crate>[1];
            foreach (var i in input)
            {
                space[0] = i;
                _sut.Put(space);
            }

            var output = _sut.ToArray();
            var expectedOutput = IntervalGeneration.Sequence(0, 5, 5, input.Length - 1, i => input[i].Payload).Concat(input.Skip(input.Length - 1));
            CollectionAssert.AreEqual(expectedOutput, output);
        }

        [TestCase(0)]
        [TestCase(10)]
        public void SingleRemove(int margin)
        {
            var i = new MappedInterval<Crate>(0, 100, MakeDummy(0));
            _sut.Put(new [] { i });

            _sut.Delete(i.IntervalStart - margin, i.IntervalEnd + margin);

            Assert.That(_sut.Count, Is.Zero);
        }

        [Test]
        public void PartialRemove()
        {
            var i = new MappedInterval<Crate>(0, 1000, MakeDummy(0));
            _sut.Put(new []{ i });

            var gapFrom = 100;
            var gapTo = 400;

            _sut.Delete(gapFrom, gapTo);

            var output = _sut.ToArray();
            Assert.That(output.Length, Is.EqualTo(2));
            Assert.That(output[0].Payload.Value, Is.EqualTo(0));
            Assert.That(output[0].IntervalStart, Is.EqualTo(i.IntervalStart));
            Assert.That(output[0].IntervalEnd, Is.EqualTo(gapFrom));
            Assert.That(output[1].Payload.Value, Is.EqualTo(0));
            Assert.That(output[1].IntervalStart, Is.EqualTo(gapTo));
            Assert.That(output[1].IntervalEnd, Is.EqualTo(i.IntervalEnd));
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(0, 5)]
        [TestCase(0, 6)]
        public void EnumerationFrom(int intervalsToSkip, int margin)
        {
            const int count = 10;
            const int duration = 5;
            const int step = 5;

            var input = IntervalGeneration.Sequence(0, duration, step, count, MakeDummy).ToArray();
            _sut.Put(input);

            var from = (duration + step) * intervalsToSkip + margin;
            var later = input.Where(i => i.IntervalEnd >= from).ToArray();

            var part = CollectFrom(_sut, from).ToArray();
            Assert.That(part.Length, Is.EqualTo(later.Length));
            CollectionAssert.AreEqual(part, later);
        }

        private static IEnumerable<MappedInterval<T>> CollectFrom<T>(IMappedIntervalsCollection<T> collection, long from)
        {
            using (var e = collection.GetEnumerator(from))
            {
                while (e.MoveNext())
                {
                    yield return e.Current;
                }
            }
        }

        private static Crate MakeDummy(int value)
        {
            return new Crate(value);
        }

    }
}