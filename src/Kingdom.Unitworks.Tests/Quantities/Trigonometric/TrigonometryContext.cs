using System;
using Kingdom.Unitworks.Dimensions;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Kingdom.Unitworks.Trigonometric
{
    internal class TrigonometryContext : ITrigonometryContext
    {
        private double _epsilon = double.NaN;

        public bool HasEpsilon
        {
            get { return !double.IsNaN(_epsilon); }
        }

        public ITrigonometryContext SetEpsilon(double value)
        {
            _epsilon = value;
            return this;
        }

        public double Epsilon
        {
            get { return _epsilon; }
        }

        //public static TrigonometryContext Create<TAngle>(double startingValue, Func<double, IQuantity> startingQtyFactory, Func<double, IQuantity> expectedQtyFactory, Func<IQuantity, IQuantity> )
        //    where TAngle : Dimension, IAngle
        //{
        //}

        private ITrigonometricPart _startingPart;

        private ITrigonometricPart _expectedPart;

        private IQuantity _actualQty;

        public ITrigonometryContext Starting(Func<ITrigonometryContext, ITrigonometricPart> starting)
        {
            Assert.That(starting, Is.Not.Null);
            _startingPart = starting(this);
            return this;
        }

        public ITrigonometryContext Expected(Func<ITrigonometryContext, ITrigonometricPart> expected)
        {
            Assert.That(expected, Is.Not.Null);
            _expectedPart = expected(this);
            return this;
        }

        public ITrigonometryContext Function(Func<IQuantity, IQuantity> function)
        {
            Assert.That(function, Is.Not.Null);
            _actualQty = function(_startingPart.Qty);
            return this;
        }

        private static Constraint CheckInfinity(IQuantity qty)
        {
            var result = double.IsPositiveInfinity(qty.Value)
                ? Is.EqualTo(double.PositiveInfinity)
                : null;

            return result ?? (double.IsNegativeInfinity(qty.Value)
                ? Is.EqualTo(double.NegativeInfinity)
                : null);
        }

        private static Constraint CheckNaN(IQuantity qty)
        {
            return double.IsNaN(qty.Value) ? Is.NaN : null;
        }

        private static double ApplyEpsilon(double value, double epsilon)
        {
            var exponent = 1;

            // Make sure we land on a sufficient exponent in order to apply the expected precision.
            while (Math.Truncate(epsilon*Math.Pow(10d, exponent++)) <= 0d)
            {
            }

            var factor = Math.Pow(10d, exponent);

            var result = Math.Round(value*factor)/factor;

            return result;
        }

        private Constraint CheckValue(IQuantity qty)
        {
            return HasEpsilon
                ? Is.EqualTo(qty.Value).Within(Epsilon)
                : Is.EqualTo(qty.Value);
        }

        public void Dispose()
        {
            var expectedQty = _expectedPart.Qty;

            Assert.That(_actualQty, Is.Not.SameAs(expectedQty));

            //TODO: really need/want IEquatable<IQuantity>
            Assert.That(_actualQty.Dimensions.AreEquivalent(_expectedPart.Qty.Dimensions));

            var actualValue = ApplyEpsilon(_actualQty.Value, Epsilon);

            Assert.That(actualValue, CheckInfinity(expectedQty) ?? CheckNaN(expectedQty) ?? CheckValue(expectedQty));
        }
    }
}
