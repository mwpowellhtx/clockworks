using System;

namespace Kingdom.Unitworks.Calculators.Fixtures
{
    internal class RightTriangleCalculatorFixture : RightTriangleCalculatorFixtureBase<RightTriangleCalculator>
    {
        internal RightTriangleCalculatorFixture(Func<double, double, double> expected,
            Func<RightTriangleCalculator, IQuantity, IQuantity, IQuantity> actual,
            params IQuantity[] quantities)
            : base(expected, actual, quantities)
        {
        }

        internal RightTriangleCalculatorFixture(Func<double, double, double, double> expected,
            Func<RightTriangleCalculator, IQuantity, IQuantity, IQuantity, IQuantity> actual,
            params IQuantity[] quantities)
            : base(expected, actual, quantities)
        {
        }
    }
}
