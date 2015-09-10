using System;
using Kingdom.Unitworks.Calculators.Trajectories.Components;
using Kingdom.Unitworks.Dimensions;
using NUnit.Framework;

namespace Kingdom.Unitworks.Calculators.Trajectories
{
    using T = Dimensions.Systems.Commons.Time;
    using L = Dimensions.Systems.SI.Length;

    public class BasicTrajectoryCalculatorTests : TrajectoryTestFixtureBase<
        TrajectoryParameters, BasicTrajectoryCalculatorFixture>
    {
        // TODO: Pick it up here ... consider how to verify these parts ...
        /// <summary>
        /// Verifies the default <see cref="ITrajectoryCalculator.Calculated"/> event behavior.
        /// See what happens with a single event instance given zero for input components.
        /// </summary>
        [Test]
        public void Verify_default_calculated_event_handler()
        {
            var m = L.Meter;
            var ms = T.Millisecond;

            var timeQty = new Quantity(1000d, ms);

            Action<TrajectoryCalculatorEventArgs> onNext = e =>
            {
                Assert.That(e, Is.Not.Null);

                Assert.That(e.Continue, Is.True);

                var results = e.Results;

                results.TryVerify(TrajectoryComponent.Z, false);

                /* We expect a Y component, but are less concerned with its actual value at this moment,
                 * only that the dimensions are correct. */

                results.Verify(TrajectoryComponent.Y)
                    .Verify(qty => Assert.That(qty.Dimensions.AreCompatible(new[] {m})));

                results.Verify(TrajectoryComponent.X).Verify(Quantity.Zero(m));
                results.Verify(TrajectoryComponent.MaxTime).Verify(Quantity.Zero(ms));
                results.Verify(TrajectoryComponent.MaxHeight).Verify(Quantity.Zero(m));
                results.Verify(TrajectoryComponent.MaxRange).Verify(Quantity.Zero(m));
            };

            using (Calculator.ObserveCalculated.Subscribe(onNext))
            {
                Calculator.Calculate(timeQty);
            }
        }
    }
}
