using System;
using NUnit.Framework;

namespace Kingdom.Clockworks
{
    public partial class SystemClockTests
    {
        /// <summary>
        /// Wraps up a nice fixture around each stacked level of the system clock.
        /// </summary>
        private class SystemClockFixture : Disposable
        {
            private readonly DateTime _originalNow;

            private readonly int _expectedCount;

            private readonly Func<DateTime> _nowFactory;

            private readonly ISystemClock _clock;

            private readonly Func<DateTime, DateTime> _checkNow;

            /// <summary>
            /// Internal Constructor
            /// </summary>
            /// <param name="originalNow"></param>
            /// <param name="systemNow"></param>
            /// <param name="checkNow"></param>
            /// <param name="expectedCount"></param>
            /// <param name="nowFactory"></param>
            internal SystemClockFixture(DateTime originalNow, DateTime systemNow,
                Func<DateTime, DateTime> checkNow, int expectedCount, Func<DateTime> nowFactory)
            {
                _originalNow = originalNow;
                _checkNow = checkNow;
                _expectedCount = expectedCount;
                _nowFactory = nowFactory;

                // Verify before and after installation.
                Verify(_checkNow(_originalNow), expectedCount - 1, nowFactory);

                _clock = SystemClock.Install(systemNow);

                Verify(_checkNow(systemNow), expectedCount, nowFactory);
            }

            /// <summary>
            /// This is the heart of the test fixture.
            /// </summary>
            /// <param name="expectedNow"></param>
            /// <param name="currentCount"></param>
            /// <param name="nowFactory"></param>
            private static void Verify(DateTime expectedNow, int currentCount, Func<DateTime> nowFactory)
            {
                var now = nowFactory();

                Assert.That(now, Is.GreaterThanOrEqualTo(expectedNow));
                Assert.That(now, Is.LessThan(expectedNow.AddMinutes(1d)));

                Assert.That(SystemClock.ClockCount, Is.EqualTo(currentCount));
            }

            #region Disposable Members

            /// <summary>
            /// Disposes this object.
            /// </summary>
            /// <param name="disposing"></param>
            protected override void Dispose(bool disposing)
            {
                if (disposing && !IsDisposed)
                {
                    // Rinse and repeat the verification.
                    _clock.Dispose();
                    Verify(_checkNow(_originalNow), _expectedCount - 1, _nowFactory);
                }

                base.Dispose(disposing);
            }

            #endregion
        }
    }
}
