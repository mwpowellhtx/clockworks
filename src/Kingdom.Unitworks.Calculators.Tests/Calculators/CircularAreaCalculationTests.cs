using System;
using Kingdom.Unitworks.Attributes;
using NUnit.Framework;

namespace Kingdom.Unitworks.Calculators
{
    using L = Dimensions.Systems.SI.Length;
    using A = Dimensions.Systems.SI.Area;

    public class CircularAreaCalculationTests : CalculatorTestFixtureBase<CircularCalculator>
    {
        /// <summary>
        /// Verifies that circular area calculation from diameter is correct.
        /// </summary>
        /// <param name="qty"></param>
        [Test]
        public void Verify_from_diameter([LengthValues] IQuantity qty)
        {
            using (new CircularCalculatorFixture<CircularCalculator>(
                qty, x => Math.PI*Math.Pow(x/2d, 2), x => x.CalculateArea,
                CircularCalculationType.Diameter, A.SquareMeter))
            {
            }
        }

        /// <summary>
        /// Verifies that circular area calculation from radius is correct.
        /// </summary>
        /// <param name="qty"></param>
        [Test]
        public void Verify_from_radius([LengthValues] IQuantity qty)
        {
            using (new CircularCalculatorFixture<CircularCalculator>(
                qty, x => Math.PI*Math.Pow(x, 2), x => x.CalculateArea,
                CircularCalculationType.Radius, A.SquareMeter))
            {
            }
        }
    }
}
