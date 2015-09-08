using System.Linq;
using Kingdom.Unitworks;
using NUnit.Framework;

namespace Kingdom.Clockworks.Timers
{
    using T = Unitworks.Dimensions.Systems.Commons.Time;

    /// <summary>
    /// Establishes a set of <see cref="SimulationTimer"/> unit tests.
    /// </summary>
    public class SimulationTimerTests : TimeableClockTestFixtureBase<
        SimulationTimer, TimerRequest, TimerElapsedEventArgs>
    {
        protected override IQuantity CalculateEstimated(ChangeType change, int steps,
            IQuantity intervalTimePerTimeQty, IQuantity timePerStepQty)
        {
            var resultQty = (Quantity) intervalTimePerTimeQty*timePerStepQty*steps;

            // Timers "increment" in the opposite direction, but the time away from zero is still the same quantity.
            var changes = new[] {ChangeType.Increment, ChangeType.Addition};

            if (changes.Any(x => (change & x) == x))
                resultQty = -resultQty;

            ////TODO: for now, we will allow negative.
            //// Cannot be less than Zero.
            //resultQty = (Quantity) Quantity.Max(Quantity.Zero(T.Millisecond), resultQty);

            return resultQty;
        }

        /// <summary>
        /// Verifies that the timer  <see cref="IMeasurableClock.ElapsedQty"/> cannot be
        /// negative. This means setting <see cref="ISimulationTimer.CannotBeNegative"/> to
        /// <see cref="bool.True"/> and verifying that increment does not advance the quantity
        /// past zero. This is specialized behavior, the default behavior for which is already
        /// tested by the test fixture base class.
        /// </summary>
        [Test]
        public void Verify_timer_cannot_be_negative()
        {
            //TODO: could expand the test cases a little bit once this is working...
            using (var clock = CreateClock())
            {
                clock.CannotBeNegative = true;

                // Two sides of the same concern.
                Assert.That(clock.CannotBeNegative, Is.True);
                Assert.That(clock.CanBeNegative, Is.False);

                /* Operators like increment or decrement will not work because of the using block.
                 * That is very interesting, I've never seen that before, but it stands to reason. */

                // Incrementing from zero starting point when it cannot be negative should yield zero.
                clock.Increment();

                var elapsedQty = clock.ElapsedQty;

                Assert.That(elapsedQty.Equals(Quantity.Zero(elapsedQty.Dimensions)));
            }
        }
    }
}
