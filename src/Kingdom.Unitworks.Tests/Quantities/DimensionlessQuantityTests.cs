using Kingdom.Unitworks.Dimensions.Systems.Commons;
using Kingdom.Unitworks.Dimensions.Systems.SI;
using NUnit.Framework;

namespace Kingdom.Unitworks
{
    using A = Area;
    using L = Length;

    public class DimensionlessQuantityTests : TestFixtureBase
    {
        [Test]
        public void Verify_that_pure_dimless_is_dimless()
        {
            var qty = new Quantity();
            Assert.That(qty.IsDimensionless);
        }

        /// <summary>
        /// Verifies when a first order derived dimensionless is actually dimensionless. This
        /// combination involves any mix of purely base units, such as <see cref="Length"/>,
        /// <see cref="Time"/>, etc.
        /// </summary>
        [Test]
        public void Verify_that_first_order_derived_dimless_is_dimless()
        {
            var derivedQty = new Quantity(default(double), L.Meter, L.Meter.Invert());
            Assert.That(derivedQty.IsDimensionless);
        }

        /// <summary>
        /// Verifies when a second order derived dimensionless is actually dimensionless. This
        /// combination involves any derived unit with a base unit, such as <see cref="Acceleration"/>,
        /// <see cref="Area"/>, etc, with something like <see cref="Time"/>, <see cref="Length"/>, etc.
        /// </summary>
        [Test]
        public void Verify_that_second_order_derived_dimless_is_dimless()
        {
            /* Also should not matter that they are the same units per se, only that the dimensions
             * themselves are considered dimensionless. This could easily be a more specialized version
             * of the same unit test. */

            var derivedQty = new Quantity(default(double), A.SquareKilometer, L.Meter.Invert(), L.Kilometer.Invert());
            Assert.That(derivedQty.IsDimensionless);
        }

        /// <summary>
        /// Should also verify when a first order derived dimensionless is not dimensionless.
        /// </summary>
        [Test]
        public void Verify_that_first_order_derived_dimless_is_not_dimless()
        {
            var derivedQty = new Quantity(default(double), L.Kilometer, L.Meter, L.Kilometer);
            Assert.That(derivedQty.IsDimensionless, Is.False);
        }

        /// <summary>
        /// Should also verify when a second order derived dimensionless is not dimensionless.
        /// </summary>
        [Test]
        public void Verify_that_second_order_derived_dimless_is_not_dimless()
        {
            var derivedQty = new Quantity(default(double), A.SquareKilometer, A.SquareKilometer);
            Assert.That(derivedQty.IsDimensionless, Is.False);
        }
    }
}
