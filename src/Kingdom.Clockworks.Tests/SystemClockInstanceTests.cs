using System;
using NUnit.Framework;

namespace Kingdom.Clockworks
{
    public class SystemClockInstanceTests : TestFixtureBase
    {
        /// <summary>
        /// When creating a standalone instance, the static overrides should still see the system clock.
        /// This is by design. In effect this means that no instances stacked to override the system clock.
        /// </summary>
        [Test]
        public void Verify_that_static_override_exposes_system_when_created_standalone()
        {
            var now = DateTime.Now;
            var newNow = new DateTime(1999, 1, 1);

            using (ISystemClock instance = new SystemClock(newNow))
            {
                Assert.That(instance.Now, Is.GreaterThanOrEqualTo(newNow));
                Assert.That(SystemClock.ClockCount, Is.EqualTo(0));
                Assert.That(SystemClock.Now, Is.GreaterThanOrEqualTo(now));
            }

            Assert.That(SystemClock.ClockCount, Is.EqualTo(0));
            Assert.That(SystemClock.Now, Is.GreaterThanOrEqualTo(now));
        }
    }
}
