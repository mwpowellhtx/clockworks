using System;
using Kingdom.Unitworks.Attributes;
using NUnit.Framework;
using Kingdom.Unitworks.Calculators.Fixtures;

namespace Kingdom.Unitworks.Calculators
{
    using L = Dimensions.Systems.SI.Length;

    public class CircularRadiusCalculationTests : CalculatorTestFixtureBase<CircularCalculator>
    {
        /// <summary>
        /// Verifies that calculating radius from diameter is correct.
        /// </summary>
        /// <param name="qty"></param>
        /// <a href="!:http://www.calculatorsoup.com/calculators/geometry-plane/circle.php" >Circle Calculator</a>
        [Test]
        public void Verify_from_diameter([LengthValues] IQuantity qty)
        {
            const CircularCalculationType type = CircularCalculationType.Diameter;

            using (new CircularCalculatorFixture(qty,
                x => x/2d, c => c.CalculateRadius, type, L.Meter))
            {
            }
        }

        /// <summary>
        /// Verifies that calculating radius from circumference is correct.
        /// </summary>
        /// <param name="qty"></param>
        /// <a href="!:http://www.calculatorsoup.com/calculators/geometry-plane/circle.php" >Circle Calculator</a>
        [Test]
        public void Verify_from_circumference([LengthValues] IQuantity qty)
        {
            const CircularCalculationType type = CircularCalculationType.Circumference;

            using (new CircularCalculatorFixture(qty,
                x => x/(2d*Math.PI), c => c.CalculateRadius, type, L.Meter))
            {
            }
        }

        /// <summary>
        /// Verifies that calculating radius from area is correct.
        /// </summary>
        /// <param name="qty"></param>
        /// <a href="!:http://www.calculatorsoup.com/calculators/geometry-plane/circle.php" >Circle Calculator</a>
        [Test]
        public void Verify_from_area([AreaValues] IQuantity qty)
        {
            const CircularCalculationType type = CircularCalculationType.Area;

            using (new CircularCalculatorFixture(qty,
                x => Math.Sqrt(x/Math.PI), c => c.CalculateRadius, type, L.Meter))
            {
            }
        }
    }
}
