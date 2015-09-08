using System;
using Kingdom.Unitworks.Attributes;
using NUnit.Framework;
using Kingdom.Unitworks.Calculators.Fixtures;

namespace Kingdom.Unitworks.Calculators
{
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
            using (new CircularCalculatorFixture(qty,
                x => Math.PI*Math.Pow(x/2d, 2), x => x.CalculateArea,
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
            using (new CircularCalculatorFixture(qty,
                x => Math.PI*Math.Pow(x, 2), x => x.CalculateArea,
                CircularCalculationType.Radius, A.SquareMeter))
            {
            }
        }

        /// <summary>
        /// Verifies that an elliptical area calcualtion from radii <paramref name="aQty"/>
        /// and <paramref name="bQty"/> is correct.
        /// </summary>
        /// <param name="aQty"></param>
        /// <param name="bQty"></param>
        [Test]
        public void Verify_from_elliptical_radii([LengthValues] IQuantity aQty, [LengthValues] IQuantity bQty)
        {
            using (new EllipticalCalculatorFixture(aQty, bQty,
                (a, b) => Math.PI*a*b, c => c.CalculateArea,
                CircularCalculationType.Area, A.SquareMeter))
            {
            }
        }
    }
}
