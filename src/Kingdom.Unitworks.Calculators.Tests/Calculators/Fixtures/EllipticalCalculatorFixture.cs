using System;
using System.Linq;
using Kingdom.Unitworks.Dimensions;
using NUnit.Framework;

namespace Kingdom.Unitworks.Calculators.Fixtures
{
    internal class EllipticalCalculatorFixture : CircularCalculatorFixtureBase<CircularCalculator>
    {
        private readonly CircularCalculationType _type;

        private readonly IQuantity _aQty;

        private readonly IQuantity _bQty;

        private readonly Func<double, double, double> _expected;

        private readonly Func<CircularCalculator, Func<IQuantity, IQuantity, IQuantity>> _actual;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aQty"></param>
        /// <param name="bQty"></param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="type"></param>
        /// <param name="dimensions"></param>
        public EllipticalCalculatorFixture(
            IQuantity aQty, IQuantity bQty,
            Func<double, double, double> expected,
            Func<CircularCalculator, Func<IQuantity, IQuantity, IQuantity>> actual,
            CircularCalculationType type,
            params IDimension[] dimensions)
            : base(dimensions)
        {
            _aQty = aQty;
            _bQty = bQty;
            _expected = expected;
            _actual = actual;
            _type = type;
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public override void Dispose()
        {
            var expectedValue = _expected(_aQty.ToBase().Value, _bQty.ToBase().Value);

            var expectedQty = new Quantity(expectedValue, Dimensions.ToArray());

            var calc = _actual(Calculator);

            Assert.That(calc, Is.Not.Null);

            var actualQty = calc(_aQty, _bQty);

            var comparer = new QuantityComparer(-4);

            Assert.That(comparer.Epsilon, Is.EqualTo(1e-4));

            Assert.That(comparer.Compare(expectedQty, actualQty), Is.EqualTo(0),
                "Given {{{0}}}, {{{1}}} and {{{2}}} expected {{{3}}} but was {{{4}}}:"
                + " values were {{{5}}} and {{{6}}}",
                _type, _aQty, _bQty, expectedQty, actualQty,
                expectedQty.Value.ToString("R"), actualQty.Value.ToString("R"));

            Console.WriteLine(
                "Given {{{0}}}, {{{1}}} and {{{2}}} expected {{{3}}} but was {{{4}}}:"
                + " values were {{{5}}} and {{{6}}}",
                _type, _aQty, _bQty, expectedQty, actualQty,
                expectedQty.Value.ToString("R"), actualQty.Value.ToString("R"));
        }
    }
}
