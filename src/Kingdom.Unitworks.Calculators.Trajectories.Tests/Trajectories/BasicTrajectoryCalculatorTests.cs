using System;
using System.Collections.Generic;
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
        private IQuantity _maxRangeQty;

        private IQuantity MaxRangeQty
        {
            get { return _maxRangeQty; }
            set
            {
                _maxRangeQty = _maxRangeQty ?? value;
                Assert.That(_maxRangeQty, Is.Not.Null);
            }
        }

        private IQuantity _maxHeightQty;

        private IQuantity MaxHeightQty
        {
            get { return _maxHeightQty; }
            set
            {
                _maxHeightQty = _maxHeightQty ?? value;
                Assert.That(_maxHeightQty, Is.Not.Null);
            }
        }

        private readonly IList<Tuple<IQuantity, IQuantity>> _trajectory
            = new List<Tuple<IQuantity, IQuantity>>();

        public override void SetUp()
        {
            base.SetUp();

            _maxRangeQty = null;
            _maxHeightQty = null;

            _trajectory.Clear();

            Continue = true;
        }

        public override void TearDown()
        {
            _trajectory.Clear();

            _maxRangeQty = null;
            _maxHeightQty = null;

            base.TearDown();
        }

        /// <summary>
        /// Gets or sets whether to Continue.
        /// </summary>
        private bool Continue { get; set; }

        /// <summary>
        /// <see cref="ITrajectoryCalculator.Calculated"/> observable handler.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNext(TrajectoryCalculatorEventArgs e)
        {
            Action<IQuantity> ofLength = qty => Assert.That(qty.Dimensions.AreCompatible(new[] {L.Meter}, true));

            MaxRangeQty = e.Results.Verify(TrajectoryComponent.MaxRange).Verify(ofLength);
            MaxHeightQty = e.Results.Verify(TrajectoryComponent.MaxHeight).Verify(ofLength);

            var xQty = e.Results.Verify(TrajectoryComponent.X).Verify(ofLength);
            var yQty = e.Results.Verify(TrajectoryComponent.Y).Verify(ofLength);

            var item = Tuple.Create(xQty, yQty);

            /* The X and Range components may be unreliable especially once aerodynamic
             * resistance is factored in. But for this purpose this is sufficient. */

            Continue = e.Continue = (Quantity) xQty < MaxRangeQty;

            _trajectory.Add(item);
        }

        /// <summary>
        /// Verifies the default <see cref="ITrajectoryCalculator.Calculated"/> event behavior.
        /// See what happens with a single event instance given zero for input components.
        /// </summary>
        /// <param name="vlaQty"></param>
        /// <param name="ivQty"></param>
        [Test]
        [Combinatorial]
        public void Verify_fully_calculated_trajectory(
            [PlanarAngleValues] IQuantity vlaQty,
            [VelocityValues] IQuantity ivQty)
        {
            Parameters.VerticalLaunchAngleQty = vlaQty;
            Parameters.InitialVelocityQty = ivQty;

            var ms = T.Millisecond;

            var t = new Quantity(100d, ms);
            var currentTimeQty = Quantity.Zero(ms);

            using (Calculator.ObserveCalculated.Subscribe(OnNext))
            {
                // Keep calculating while we are allowed to do so.
                while (Continue) Calculator.Calculate(currentTimeQty += t);
            }

            // Verify that we have a valid trajectory.
            _trajectory.Verify(MaxHeightQty);
        }
    }
}
