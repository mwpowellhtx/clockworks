using System;
using NUnit.Framework;

namespace Kingdom.Unitworks
{
    using L = Dimensions.Systems.SI.Length;

    /// <summary>
    /// A and <see cref="B"/> are both dimensionless and equal. <see cref="C"/> is dimensionless
    /// and half of A. <see cref="E"/>, <see cref="F"/>, and <see cref="G"/> are all dimensional
    /// using a modest <see cref="L"/> dimension for simplicity, although any dimension should
    /// work. <see cref="E"/> and <see cref="F"/> are both equal but in different <see cref="L"/>
    /// units, while <see cref="G"/> is less than <see cref="E"/>.
    /// </summary>
    public abstract class DimensionalQuantityLogicalTestFixtureBase
        : DimensionlessQuantityBinaryOperatorTestFixtureBase<bool, bool>
    {
        protected IQuantity B { get; set; }

        protected IQuantity C { get; set; }

        protected IQuantity D { get; set; }

        protected IQuantity E { get; set; }

        protected IQuantity F { get; set; }

        /// <summary>
        /// CValue: <see cref="TestFixtureBase.Value"/> divided by 2.
        /// </summary>
        protected const double CValue = Value/2d;

        /// <summary>
        /// DValue: <see cref="TestFixtureBase.Value"/> times <see cref="L.MetersPerKilometer"/>.
        /// </summary>
        protected const double DValue = Value*L.MetersPerKilometer;

        protected override void InitializeQuantities()
        {
            VerifyDimensionlessQuantity(A = new Quantity(Value));
            VerifyDimensionlessQuantity(B = new Quantity(Value));
            VerifyDimensionlessQuantity(C = new Quantity(CValue), CValue);
            VerifyDimensionalQuantity(D = new Quantity(DValue, L.Meter), DValue);
            VerifyDimensionalQuantity(E = new Quantity(Value, L.Kilometer));
            VerifyDimensionalQuantity(F = new Quantity(Value, L.Meter));
        }

        protected static void VerifyDimensionalQuantity(IQuantity quantity, double expectedValue = Value)
        {
            Assert.That(quantity, Is.Not.Null);
            Assert.That(quantity.IsDimensionless, Is.False);
            Assert.That(quantity.Value, Is.EqualTo(expectedValue));

            Assert.That(quantity.Dimensions, Is.Not.Null);
            CollectionAssert.IsNotEmpty(quantity.Dimensions);
        }

        private void VerifyResult(IQuantity a, IQuantity b, bool actual, bool? expected = null)
        {
            var aBase = a.ToBase();
            var bBase = b.ToBase();

            if (!expected.HasValue)
                expected = CalculateExpectedResult(aBase.Value, bBase.Value);

            Assert.That(expected.HasValue);
            Assert.That(actual, Is.EqualTo(expected));
        }

        private void VerifyResult(IQuantity a, double b, bool actual, bool? expected = null)
        {
            var aBase = a.ToBase();

            if (!expected.HasValue)
                expected = CalculateExpectedResult(aBase.Value, b);

            Assert.That(expected.HasValue);
            Assert.That(actual, Is.EqualTo(expected));
        }

        private void VerifyResult(double a, IQuantity b, bool actual, bool? expected = null)
        {
            var bBase = b.ToBase();

            if (!expected.HasValue)
                expected = CalculateExpectedResult(a, bBase.Value);

            Assert.That(expected.HasValue);
            Assert.That(actual, Is.EqualTo(expected));
        }

        protected void Verify<TX, TY>(TX x, TY y, bool? expected = null)
            where TX : IQuantity
            where TY : IQuantity
        {
            var qty = (Quantity) Convert.ChangeType(x, typeof (Quantity));
            var result = qty.InvokeOperator<Quantity, bool>(Operator, x, y);
            VerifyResult(x, y, result, expected);
        }

        protected void Verify<TX>(IQuantity x, double y, bool? expected = null)
            where TX : IQuantity
        {
            var qty = (TX) Convert.ChangeType(x, typeof (TX));
            var result = qty.InvokeOperator<TX, bool>(Operator, qty, y);
            //TODO: consider whether this ought not throw an exception for a dimensional compared with dimensionless...
            VerifyResult(x, y, result, expected ?? (x.IsDimensionless ? null : (bool?) false));
        }

        protected void Verify<TY>(double x, IQuantity y, bool? expected = null)
            where TY : IQuantity
        {
            var qty = (TY) Convert.ChangeType(y, typeof (TY));
            var result = qty.InvokeOperator<TY, bool>(Operator, x, qty);
            //TODO: ditto above...
            VerifyResult(x, y, result, expected ?? (y.IsDimensionless ? null : (bool?) false));
        }
    }
}
