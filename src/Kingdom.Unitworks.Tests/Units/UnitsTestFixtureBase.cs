using System;
using NUnit.Framework;

namespace Kingdom.Unitworks.Units
{
    public abstract class UnitsTestFixtureBase : TestFixtureBase
    {
    }

    public abstract class UnitsTestFixtureBase<TDimension> : UnitsTestFixtureBase
    {
        protected abstract double GetFactor(TDimension unit);

        /// <summary>
        /// Returns the <paramref name="value"/> converted to the base unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected double ConvertToBase(TDimension unit, double value)
        {
            return value*GetFactor(unit);
        }

        /// <summary>
        /// Returns the <paramref name="value"/> converted from the base unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected double ConvertFromBase(TDimension unit, double value)
        {
            return value/GetFactor(unit);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal static class QuantityBaseExtensionMethods
    {
        /// <summary>
        /// Performs basic verification on the <paramref name="quantity"/>.
        /// </summary>
        /// <typeparam name="TDimension"></typeparam>
        /// <typeparam name="TQuantity"></typeparam>
        /// <param name="quantity"></param>
        /// <param name="unit"></param>
        /// <param name="value"></param>
        /// <param name="epsilon"></param>
        /// <param name="verify"></param>
        /// <returns>The given <paramref name="quantity"/>.</returns>
        public static TQuantity Verify<TDimension, TQuantity>(this TQuantity quantity, TDimension? unit = null,
            double value = default(double), double? epsilon = null, Action<TQuantity> verify = null)
            where TQuantity : QuantityBase<TDimension, double>, new()
            where TDimension : struct
        {
            unit = unit ?? (new TQuantity() as IQuantity<TDimension>).BaseUnit;
            verify = verify ?? (x => { });

            Assert.That(quantity, Is.Not.Null);
            Assert.That(quantity.Unit, Is.EqualTo(unit.Value));

            Assert.That(quantity.Value, epsilon.HasValue
                ? Is.EqualTo(value).Within(epsilon.Value)
                : Is.EqualTo(value));

            verify(quantity);

            return quantity;
        }
    }
}
