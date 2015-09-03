using System;
using System.Linq;
using Kingdom.Unitworks.Dimensions;
using NUnit.Framework;

namespace Kingdom.Unitworks.Calculators.Fixtures
{
    internal class CircularCalculatorFixture : CircularCalculatorFixtureBase<CircularCalculator>
    {
        private readonly CircularCalculationType _type;

        private readonly IQuantity _qty;

        private readonly Func<double, double> _expected;

        private readonly Func<CircularCalculator, Func<IQuantity, CircularCalculationType, IQuantity>> _actual;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="type"></param>
        /// <param name="dimensions"></param>
        public CircularCalculatorFixture(
            IQuantity qty,
            Func<double, double> expected,
            Func<CircularCalculator, Func<IQuantity, CircularCalculationType, IQuantity>> actual,
            CircularCalculationType type,
            params IDimension[] dimensions)
            : base(dimensions)
        {
            _type = type;
            _qty = qty;
            _actual = actual;
            _expected = expected;
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public override void Dispose()
        {
            var expectedValue = _expected(_qty.ToBase().Value);

            var expectedQty = new Quantity(expectedValue, Dimensions.ToArray());

            var calc = _actual(Calculator);

            Assert.That(calc, Is.Not.Null);

            var actualQty = calc(_qty, _type);

            var comparer = new QuantityComparer(-4);

            Assert.That(comparer.Epsilon, Is.EqualTo(1e-4));

            Assert.That(comparer.Compare(actualQty, expectedQty), Is.EqualTo(0),
                "Given {{{0}}} and {{{1}}} expected {{{2}}} but was {{{3}}}:"
                + " values {{{4}}} and {{{5}}}",
                _type, _qty, expectedQty, actualQty,
                expectedValue.ToString("R"), actualQty.Value.ToString("R"));

            Console.WriteLine(
                "Given {{{0}}} and {{{1}}} expected {{{2}}} actual result {{{3}}}:"
                + " values {{{4}}} and {{{5}}}",
                _type, _qty, expectedQty, actualQty,
                expectedValue.ToString("R"), actualQty.Value.ToString("R"));
        }
    }
}
