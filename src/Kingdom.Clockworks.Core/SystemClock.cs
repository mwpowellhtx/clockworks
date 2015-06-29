using System;
using System.Collections.Generic;
using System.Linq;

namespace Kingdom.Clockworks
{
    //TODO: with moving timer that calculates current time on demand? or in what gradation? i.e. increment?
    //TODO: put in some "simple" unit tests around this one...
    //TODO: should be more loosely coupled than a static concern? probably...
    /// <summary>
    /// Provides a convenience way to override the system clock.
    /// </summary>
    public class SystemClock : Disposable, ISystemClock
    {
        /// <summary>
        /// Difference to be added to the system clock.
        /// </summary>
        private readonly TimeSpan _difference;

        /// <summary>
        /// Private Default Constructor
        /// </summary>
        /// <param name="newNow">The new now.</param>
        private SystemClock(DateTime newNow)
        {
            //TODO: we may need to be concerned about date/time components? but I do not think that this is necessary ...
            //TODO: make sure this is always in local time ... TBD
            _difference = newNow - DateTime.Now;
        }

        //TODO: thread local? should they "stack"? that is, we have more than one at a time?
        //TODO: does not necessarily need to take "deque" as a dependency here... a simple(r) "list" would be sufficient ... ?
        private static readonly IList<ISystemClock> Clocks = new List<ISystemClock>();

        /// <summary>
        /// Pushes a new <see cref="ISystemClock"/> instance onto the deque and returns.
        /// For best results this should be used in a using block.
        /// </summary>
        /// <param name="newNow"></param>
        /// <returns></returns>
        public static ISystemClock Install(DateTime newNow)
        {
            lock (Clocks)
            {
                Clocks.Insert(0, new SystemClock(newNow));
                return Clocks.First();
            }
        }

        /// <summary>
        /// Returns the <see cref="DateTime"/> corresponding to <see cref="DateTime.Now"/>
        /// plus the <see cref="_difference"/>.
        /// </summary>
        /// <returns></returns>
        private DateTime GetNow()
        {
            return DateTime.Now + _difference;
        }

        /// <summary>
        /// Returns the <see cref="DateTime"/> corresponding to <see cref="DateTime.UtcNow"/>
        /// plus the <see cref="_difference"/>.
        /// </summary>
        /// <returns></returns>
        private DateTime GetUtcNow()
        {
            return DateTime.UtcNow + _difference;
        }

        /// <summary>
        /// Gets the overridden Now as a local time.
        /// </summary>
        DateTime ISystemClock.Now
        {
            get { return GetNow(); }
        }

        /// <summary>
        /// Gets the overridden UtcNow as Coordinated Universal Time (UTC).
        /// </summary>
        DateTime ISystemClock.UtcNow
        {
            get { return GetUtcNow(); }
        }

        /// <summary>
        /// Returns the <see cref="DateTime"/> corresponding to <see cref="DateTime.Now"/>
        /// plus the <see cref="_difference"/>.
        /// </summary>
        /// <param name="aClock"></param>
        /// <returns></returns>
        private static DateTime GetNow(SystemClock aClock)
        {
            return aClock == null ? DateTime.Now : aClock.GetNow();
        }

        /// <summary>
        /// Returns the <see cref="DateTime"/> corresponding to <see cref="DateTime.UtcNow"/>
        /// plus the <see cref="_difference"/>.
        /// </summary>
        /// <param name="aClock"></param>
        /// <returns></returns>
        private static DateTime GetUtcNow(SystemClock aClock)
        {
            return aClock == null ? DateTime.UtcNow : aClock.GetUtcNow();
        }

        /// <summary>
        /// Gets a <see cref="DateTime"/> object that overrides the current date and time
        /// on this computer, expressed as the local time.
        /// </summary>
        /// <returns>An object whose value is the overridden local date and time.</returns>
        public static DateTime Now
        {
            get
            {
                lock (Clocks)
                {
                    // This doesn't look like much but this is thinning the interface as much as possible.
                    return Clocks.Any() ? GetNow(Clocks.First() as SystemClock) : GetNow(null);
                }
            }
        }

        /// <summary>
        /// Gets a <see cref="DateTime"/> object that overrides the current date and time
        /// on this computer, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <returns>An object whose value is the overridden UTC date and time.</returns>
        public static DateTime UtcNow
        {
            get
            {
                lock (Clocks)
                {
                    // This doesn't look like much but this is thinning the interface as much as possible.
                    return Clocks.Any() ? GetUtcNow(Clocks.First() as SystemClock) : GetUtcNow(null);
                }
            }
        }

        /// <summary>
        /// Gets the ClockCount.
        /// </summary>
        public static int ClockCount
        {
            get { lock (Clocks) return Clocks.Count; }
        }

        /// <summary>
        /// Disposes the front most clock from the  <see cref="Clocks"/> deque.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed)
            {
                lock (Clocks)
                {
                    //TODO: that's assuming that we're talking about "contains" means "it's the front one" ...
                    if (!Clocks.Contains(this)) return;
                    Clocks.Remove(this);
                }
            }

            base.Dispose(disposing);
        }
    }
}
