using System;
using Kingdom.Unitworks.Attributes;
using NUnit.Framework;
using Kingdom.Unitworks.Calculators.Fixtures;

namespace Kingdom.Unitworks.Calculators
{
    using L = Dimensions.Systems.SI.Length;

    public class CircularDiameterCalculationTests : CalculatorTestFixtureBase<CircularCalculator>
    {
        /// <summary>
        /// Verifies that calculating diameter from radius is correct.
        /// </summary>
        /// <param name="qty"></param>
        [Test]
        public void Verify_from_radius([LengthValues] IQuantity qty)
        {
            const CircularCalculationType type = CircularCalculationType.Radius;

            using (new CircularCalculatorFixture(qty,
                x => 2d*x, c => c.CalculateDiameter, type, L.Meter))
            {
            }
        }

        /// <summary>
        /// Verifies that calculating diameter from circumference is correct.
        /// </summary>
        /// <param name="qty"></param>
        [Test]
        public void Verify_from_circumference([LengthValues] IQuantity qty)
        {
            const CircularCalculationType type = CircularCalculationType.Circumference;

            using (new CircularCalculatorFixture(qty,
                x => 2d*(x/(2d*Math.PI)), c => c.CalculateDiameter, type, L.Meter))
            {
            }
        }

        /// <summary>
        /// Verifies that calculating diameter from area is correct.
        /// </summary>
        /// <param name="qty"></param>
        [Test]
        public void Verify_from_area([AreaValues] IQuantity qty)
        {
            const CircularCalculationType type = CircularCalculationType.Area;

            using (new CircularCalculatorFixture(qty,
                x => 2d*Math.Sqrt(x/Math.PI), c => c.CalculateDiameter, type, L.Meter))
            {
            }
        }
    }
}
