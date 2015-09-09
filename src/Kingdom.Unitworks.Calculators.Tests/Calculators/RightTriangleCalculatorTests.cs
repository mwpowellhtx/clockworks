using System;
using Kingdom.Unitworks.Attributes;
using Kingdom.Unitworks.Calculators.Fixtures;
using NUnit.Framework;

namespace Kingdom.Unitworks.Calculators
{
    using L = Dimensions.Systems.SI.Length;
    using Theta = Dimensions.Systems.SI.PlanarAngle;

    public class RightTriangleCalculatorTests
        : CalculatorTestFixtureBase<RightTriangleCalculator>
    {
        /// <summary>
        /// Verifies that the hypotenuse is correct.
        /// </summary>
        /// <param name="aQty"></param>
        /// <param name="bQty"></param>
        [Test]
        [Combinatorial]
        public void Verify_hypotenuse_is_correct(
            [LengthValues] IQuantity aQty,
            [LengthValues] IQuantity bQty)
        {
            using (new RightTriangleCalculatorFixture((a, b) => Math.Sqrt(a*a + b*b),
                (calc, a, b) => calc.CalculateHypotenuse(a, b), aQty, bQty)
            {
                ExpectedQty = c => new Quantity(c, L.Meter)
            })
            {
            }
        }

        /// <summary>
        /// Verifies that the non hypotenuse is correct.
        /// </summary>
        /// <param name="cQty"></param>
        /// <param name="abQty"></param>
        [Test]
        [Combinatorial]
        public void Verify_non_hypotenuse_is_correct(
            [LengthValues] IQuantity cQty,
            [LengthValues] IQuantity abQty)
        {
            using (new RightTriangleCalculatorFixture((c, ab) => Math.Sqrt(c*c - ab*ab),
                (calc, c, ab) => calc.CalculateNonHypotenuse(c, ab), cQty, abQty)
            {
                ExpectedQty = ba => new Quantity(ba, L.Meter)
            })
            {
            }
        }

        /// <summary>
        /// Verifies that the Alpha, α, angle calculates correctly, provided
        /// <paramref name="aQty"/> and <paramref name="bQty"/>.
        /// </summary>
        /// <param name="aQty"></param>
        /// <param name="bQty"></param>
        [Test]
        [Combinatorial]
        public void Verify_Alpha_angle_given_a_b_is_correct(
            [LengthValues] IQuantity aQty, [LengthValues] IQuantity bQty)
        {
            using (new RightTriangleCalculatorFixture((a, b) => Math.Atan(a/b),
                (calc, a, b) => calc.CalculateAngleAlpha(a, b), aQty, bQty)
            {
                // ReSharper disable once InconsistentNaming
                ExpectedQty = Alpha => new Quantity(Alpha, Theta.Radian)
            })
            {
            }
        }

        /// <summary>
        /// Verifies that the Alpha, α, angle calculates correctly, provided
        /// <paramref name="aQty"/> and <paramref name="cQty"/>.
        /// </summary>
        /// <param name="aQty"></param>
        /// <param name="cQty"></param>
        [Test]
        [Combinatorial]
        public void Verify_Alpha_angle_given_a_c_is_correct(
            [LengthValues] IQuantity aQty, [LengthValues] IQuantity cQty)
        {
            // Only works for c values that are greater than a.
            if ((Quantity) cQty > aQty) return;

            using (new RightTriangleCalculatorFixture((a, c) => Math.Asin(a/c),
                (calc, a, c) => calc.CalculateAngleAlpha(a, cQty: c), aQty, cQty)
            {
                // ReSharper disable once InconsistentNaming
                ExpectedQty = Alpha => new Quantity(Alpha, Theta.Radian)
            })
            {
            }
        }

        /// <summary>
        /// Verifies that the Alpha, α, angle calculates correctly, provided
        /// <paramref name="bQty"/> and <paramref name="cQty"/>.
        /// </summary>
        /// <param name="bQty"></param>
        /// <param name="cQty"></param>
        [Test]
        [Combinatorial]
        public void Verify_Alpha_angle_given_b_c_is_correct(
            [LengthValues] IQuantity bQty, [LengthValues] IQuantity cQty)
        {
            using (new RightTriangleCalculatorFixture((b, c) => Math.Acos(b/c),
                (calc, b, c) => calc.CalculateAngleAlpha(bQty: b, cQty: c), bQty, cQty)
            {
                // ReSharper disable once InconsistentNaming
                ExpectedQty = Alpha => new Quantity(Alpha, Theta.Radian)
            })
            {
            }
        }

        /// <summary>
        /// Verifies that the Beta, ß, angle calculates correctly, provided
        /// <paramref name="aQty"/> and <paramref name="bQty"/>.
        /// </summary>
        /// <param name="aQty"></param>
        /// <param name="bQty"></param>
        [Test]
        [Combinatorial]
        public void Verify_Beta_angle_given_a_b_is_correct(
            [LengthValues] IQuantity aQty, [LengthValues] IQuantity bQty)
        {
            using (new RightTriangleCalculatorFixture((a, b) => Math.Atan(b/a),
                (calc, a, b) => calc.CalculateAngleBeta(a, b), aQty, bQty)
            {
                // ReSharper disable once InconsistentNaming
                ExpectedQty = Beta => new Quantity(Beta, Theta.Radian)
            })
            {
            }
        }

        /// <summary>
        /// Verifies that the Beta, ß, angle calculates correctly, provided
        /// <paramref name="aQty"/> and <paramref name="cQty"/>.
        /// </summary>
        /// <param name="aQty"></param>
        /// <param name="cQty"></param>
        [Test]
        [Combinatorial]
        public void Verify_Beta_angle_given_a_c_is_correct(
            [LengthValues] IQuantity aQty, [LengthValues] IQuantity cQty)
        {
            // Only works for c values that are greater than a.
            if ((Quantity) cQty > aQty) return;

            using (new RightTriangleCalculatorFixture((a, c) => Math.Acos(a/c),
                (calc, a, c) => calc.CalculateAngleBeta(a, cQty: c), aQty, cQty)
            {
                // ReSharper disable once InconsistentNaming
                ExpectedQty = Beta => new Quantity(Beta, Theta.Radian)
            })
            {
            }
        }

        /// <summary>
        /// Verifies that the Beta, ß, angle calculates correctly, provided
        /// <paramref name="bQty"/> and <paramref name="cQty"/>.
        /// </summary>
        /// <param name="bQty"></param>
        /// <param name="cQty"></param>
        [Test]
        [Combinatorial]
        public void Verify_Beta_angle_given_b_c_is_correct(
            [LengthValues] IQuantity bQty, [LengthValues] IQuantity cQty)
        {
            using (new RightTriangleCalculatorFixture((b, c) => Math.Asin(b/c),
                (calc, b, c) => calc.CalculateAngleBeta(bQty: b, cQty: c), bQty, cQty)
            {
                // ReSharper disable once InconsistentNaming
                ExpectedQty = Beta => new Quantity(Beta, Theta.Radian)
            })
            {
            }
        }
    }
}
