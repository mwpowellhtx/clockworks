using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Kingdom.Clockworks
{
    //TODO: refactor "SystemClock" to a "clock works" library, with unit tests, and nuspec...
    public partial class SystemClockTests : TestFixtureBase
    {
        /// <summary>
        /// Gets or sets the UtcOffset.
        /// </summary>
        private TimeSpan UtcOffset { get; set; }

        /// <summary>
        /// Sets up the test fixture prior to running the tests.
        /// </summary>
        public override void TestFixtureSetUp()
        {
            base.TestFixtureSetUp();

            /* TODO: could use: http://msdn.microsoft.com/en-us/library/system.timezone.getutcoffset.aspx
             * But I'm not sure it's necessary if we do the simple math. */
            UtcOffset = DateTime.UtcNow - DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkNow"></param>
        /// <param name="nowFactory"></param>
        /// <param name="nows"></param>
        /// <returns></returns>
        private static IEnumerable<Func<SystemClockFixture>> GetFixtureFactories(
            Func<DateTime, DateTime> checkNow, Func<DateTime> nowFactory,
            params DateTime[] nows)
        {
            Assert.That(nows, Has.Length.GreaterThanOrEqualTo(2));

            for (var index = 0; index < nows.Length - 1; index++)
            {
                var i = index;
                // Basically the fixture does all the heavy lifting, so just factory create some in series.
                yield return () => new SystemClockFixture(nows[i], nows[i + 1], checkNow, i + 1, nowFactory);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nows"></param>
        /// <returns></returns>
        private static IEnumerable<Func<SystemClockFixture>> GetFixtureFactories(params DateTime[] nows)
        {
            return GetFixtureFactories(x => x, () => SystemClock.Now, nows);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        private DateTime CheckNowWithUtcAdjustment(DateTime now)
        {
            return now + UtcOffset;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static DateTime UtcNowFactory()
        {
            return SystemClock.UtcNow;
        }

        /// <summary>
        /// Verifies the fixtures by disposing each of them.
        /// </summary>
        /// <param name="fixtureFactories"></param>
        private static void Verify(params Func<SystemClockFixture>[] fixtureFactories)
        {
            // Verify using at least one fixture.
            Assert.That(fixtureFactories, Has.Length.GreaterThanOrEqualTo(1));

            /* This is about as simple as it gets to stack a series of these on top of one another,
             * considering the fixtures assert as they are initialized then dispose in reverse order. */

            var fixtures = fixtureFactories.Select(f => f()).Reverse().ToArray();

            foreach (var f in fixtures) f.Dispose();
        }

        /// <summary>
        /// One system clock without Utc.
        /// </summary>
        [Test]
        public void Verify_once()
        {
            Verify(GetFixtureFactories(
                DateTime.Now,
                new DateTime(1999, 1, 1)
                ).ToArray());
        }

        /// <summary>
        /// Two system clocks without Utc.
        /// </summary>
        [Test]
        public void Verify_twice()
        {
            Verify(GetFixtureFactories(
                DateTime.Now,
                new DateTime(1999, 1, 1),
                new DateTime(1899, 1, 1)
                ).ToArray());
        }

        /// <summary>
        /// Three (times a lady) system clock without Utc.
        /// </summary>
        [Test]
        public void Verify_three_times_a_lady()
        {
            Verify(GetFixtureFactories(
                DateTime.Now,
                new DateTime(1999, 1, 1),
                new DateTime(1899, 1, 1),
                new DateTime(1799, 1, 1)
                ).ToArray());
        }

        /// <summary>
        /// One system clock with Utc.
        /// </summary>
        [Test]
        public void Verify_once_utc()
        {
            Verify(GetFixtureFactories(
                CheckNowWithUtcAdjustment,
                UtcNowFactory,
                DateTime.Now,
                new DateTime(1999, 1, 1)
                ).ToArray());
        }

        /// <summary>
        /// Two system clocks with Utc.
        /// </summary>
        [Test]
        public void Verify_twice_utc()
        {
            Verify(GetFixtureFactories(
                CheckNowWithUtcAdjustment,
                UtcNowFactory,
                DateTime.Now,
                new DateTime(1999, 1, 1),
                new DateTime(1899, 1, 1)
                ).ToArray());
        }

        /// <summary>
        /// Three (times a lady) system clocks with Utc.
        /// </summary>
        [Test]
        public void Verify_three_times_a_lady_utc()
        {
            Verify(GetFixtureFactories(
                CheckNowWithUtcAdjustment,
                UtcNowFactory,
                DateTime.Now,
                new DateTime(1999, 1, 1),
                new DateTime(1899, 1, 1),
                new DateTime(1799, 1, 1)
                ).ToArray());
        }
    }
}
