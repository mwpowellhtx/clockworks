using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Kingdom.Unitworks.Calculators.Fixtures
{
    internal abstract class RightTriangleCalculatorFixtureBase<TCalculator>
        : CalculatorFixtureBase<TCalculator>
        where TCalculator : RightTriangleCalculator, new()
    {
        private readonly List<IQuantity> _quantities = new List<IQuantity>();

        protected IEnumerable<IQuantity> Quantities
        {
            get { return new ReadOnlyCollection<IQuantity>(_quantities); }
        }

        /// <summary>
        /// 
        /// </summary>
        protected readonly Func<double, double, double> TwoArgExpected;

        /// <summary>
        /// 
        /// </summary>
        protected readonly Func<TCalculator, IQuantity, IQuantity, IQuantity> TwoArgActual;

        /// <summary>
        /// 
        /// </summary>
        protected readonly Func<double, double, double, double> ThreeArgExpected;

        /// <summary>
        /// 
        /// </summary>
        protected readonly Func<TCalculator, IQuantity, IQuantity, IQuantity, IQuantity> ThreeArgActual;

        private RightTriangleCalculatorFixtureBase(params IQuantity[] quantities)
        {
            _quantities.AddRange(quantities);
        }

        protected RightTriangleCalculatorFixtureBase(Func<double, double, double> expected,
            Func<TCalculator, IQuantity, IQuantity, IQuantity> actual,
            params IQuantity[] quantities)
            : this(quantities)
        {
            Assert.That(!(expected == null || actual == null));
            Assert.That(quantities, Has.Length.EqualTo(2));
            TwoArgExpected = expected;
            TwoArgActual = actual;
            ThreeArgActual = null;
            ThreeArgExpected = null;
        }

        protected RightTriangleCalculatorFixtureBase(Func<double, double, double, double> expected,
            Func<TCalculator, IQuantity, IQuantity, IQuantity, IQuantity> actual,
            params IQuantity[] quantities)
            : this(quantities)
        {
            Assert.That(!(expected == null || actual == null));
            Assert.That(quantities, Has.Length.EqualTo(3));
            ThreeArgExpected = expected;
            ThreeArgActual = actual;
            TwoArgActual = null;
            TwoArgExpected = null;
        }

        private Func<double, IQuantity> _expectedQty;

        internal Func<double, IQuantity> ExpectedQty
        {
            get { return _expectedQty; }
            set
            {
                Assert.That(value, Is.Not.Null);
                _expectedQty = value;
            }
        }

        private bool IsTwoArgs
        {
            get
            {
                return !(TwoArgActual == null || TwoArgExpected == null)
                       && _quantities.Count == 2;
            }
        }

        private bool IsThreeArgs
        {
            get
            {
                return !(ThreeArgActual == null || ThreeArgExpected == null)
                       && _quantities.Count == 3;
            }
        }

        private bool? DisposeTwoArgs()
        {
            if (!IsTwoArgs) return null;

            var i = -1;
            var quantities = Quantities.ToArray();

            var xQty = quantities.ElementAt(++i);
            var yQty = quantities.ElementAt(++i);

            var expectedValue = TwoArgExpected(xQty.ToBase().Value, yQty.ToBase().Value);

            var expectedQty = ExpectedQty(expectedValue);

            var actualQty = TwoArgActual(Calculator, xQty, yQty);

            Assert.That(Comparer.Compare(expectedQty, actualQty), Is.EqualTo(0),
                "Expected {{{0}}} but was {{{1}}}: x = {{{2}}} y = {{{3}}}.",
                expectedQty, actualQty, xQty, yQty);

            return true;
        }

        private bool? DisposeThreeArgs()
        {
            if (!IsThreeArgs) return null;

            var i = -1;
            var quantities = Quantities.ToArray();

            var xQty = quantities.ElementAt(++i);
            var yQty = quantities.ElementAt(++i);
            var zQty = quantities.ElementAt(++i);

            var expectedValue = ThreeArgExpected(xQty.ToBase().Value,
                yQty.ToBase().Value, zQty.ToBase().Value);

            var expectedQty = ExpectedQty(expectedValue);

            var actualQty = ThreeArgActual(Calculator, xQty, yQty, zQty);

            Assert.That(Comparer.Compare(expectedQty, actualQty), Is.EqualTo(0),
                "Expected {{{0}}} but was {{{1}}}: x = {{{2}}} y = {{{3}}} z = {{{4}}}.",
                expectedQty, actualQty, xQty, yQty, zQty);

            Console.WriteLine("Evaluating: {{{0}}}", ThreeArgActual);

            return true;
        }

        public override void Dispose()
        {
            var disposed = DisposeTwoArgs() ?? DisposeThreeArgs() ?? false;
            Assert.That(disposed);
        }
    }
}
