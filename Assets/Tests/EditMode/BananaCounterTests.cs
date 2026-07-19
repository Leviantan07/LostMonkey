using System;
using LostMonkey.Core;
using NUnit.Framework;

namespace LostMonkey.Tests
{
    public class BananaCounterTests
    {
        [Test]
        public void NewCounter_StartsAtZero()
        {
            var counter = new BananaCounter(3);
            Assert.AreEqual(0, counter.Collected);
            Assert.AreEqual(3, counter.Total);
            Assert.IsFalse(counter.IsComplete);
        }

        [Test]
        public void Collect_IncrementsUpToTotal()
        {
            var counter = new BananaCounter(2);
            Assert.IsTrue(counter.Collect());
            Assert.AreEqual(1, counter.Collected);
            Assert.IsTrue(counter.Collect());
            Assert.AreEqual(2, counter.Collected);
            Assert.IsTrue(counter.IsComplete);
        }

        [Test]
        public void Collect_BeyondTotal_ReturnsFalseAndDoesNotOvercount()
        {
            var counter = new BananaCounter(1);
            Assert.IsTrue(counter.Collect());
            Assert.IsFalse(counter.Collect());
            Assert.AreEqual(1, counter.Collected);
        }

        [Test]
        public void ZeroTotal_IsImmediatelyComplete()
        {
            var counter = new BananaCounter(0);
            Assert.IsTrue(counter.IsComplete);
            Assert.IsFalse(counter.Collect());
        }

        [Test]
        public void NegativeTotal_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new BananaCounter(-1));
        }
    }
}
