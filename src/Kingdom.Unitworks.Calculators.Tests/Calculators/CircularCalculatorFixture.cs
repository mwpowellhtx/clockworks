using System;
using System.Collections.Generic;
using System.Linq;
using Kingdom.Unitworks.Dimensions;
using NUnit.Framework;

namespace Kingdom.Unitworks.Calculators
{
    internal class CircularCalculatorFixture<TCalculator> : IDisposable
        where TCalculator : class, ICalculator, new()
    {
        private readonly TCalculator _calculator;

        private readonly CircularCalculationType _type;

        private readonly IQuantity _qty;

        private readonly IEnumerable<IDimension> _dimensions;

        private readonly Func<double, double> _expected;

        private readonly Func<TCalculator, Func<IQuantity, CircularCalculationType, IQuantity>> _actual;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="type"></param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="dimensions"></param>
        public CircularCalculatorFixture(
            IQuantity qty,
            Func<double, double> expected,
            Func<TCalculator, Func<IQuantity, CircularCalculationType, IQuantity>> actual,
            CircularCalculationType type,
            params IDimension[] dimensions)
        {
            _qty = qty;
            _actual = actual;
            _expected = expected;
            _dimensions = dimensions;
            _type = type;
            _calculator = new TCalculator();
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            var expectedQty = new Quantity(_expected(_qty.ToBase().Value), _dimensions.ToArray());

            var actualQty = _actual(_calculator)(_qty, _type);

            Assert.That(actualQty.Equals(expectedQty), "Calculated radius expected {{{0}}} but was {{{1}}}",
                expectedQty, actualQty);

            Console.WriteLine("Given {{{0}}} {{{1}}} expected radius {{{2}}} actual result {{{3}}}",
                _type, _qty, expectedQty, actualQty);
        }
    }
}
