using System;
using Kingdom.Unitworks.Attributes;
using NUnit.Framework;
using Kingdom.Unitworks.Calculators.Fixtures;

namespace Kingdom.Unitworks.Calculators
{
    using L = Dimensions.Systems.SI.Length;

    public class CircularCircumferenceCalculationTests : CalculatorTestFixtureBase<CircularCalculator>
    {
        /// <summary>
        /// Verifies that circular circumference calculation from diameter is correct.
        /// </summary>
        /// <param name="qty"></param>
        [Test]
        public void Verify_from_diameter([LengthValues] IQuantity qty)
        {
            using (new CircularCalculatorFixture(qty,
                x => Math.PI*x, x => x.CalculateCircumference,
                CircularCalculationType.Diameter, L.Meter))
            {
            }
        }

        /// <summary>
        /// Verifies that circular circumference calculation from radius is correct.
        /// </summary>
        /// <param name="qty"></param>
        [Test]
        public void Verify_from_radius([LengthValues] IQuantity qty)
        {
            using (new CircularCalculatorFixture(qty,
                x => 2d*Math.PI*x, x => x.CalculateCircumference,
                CircularCalculationType.Radius, L.Meter))
            {
            }
        }
    }
}
