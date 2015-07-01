using System;
using NUnit.Framework;

namespace Kingdom.Clockworks.Units
{
    /// <summary>
    /// Time quantity test fixture.
    /// </summary>
    public class TimeQuantityTests : ClockworksTestFixtureBase
    {
        /// <summary>
        /// Tests that the <see cref="TimeQuantity"/> unit converter has been initialized correctly.
        /// </summary>
        [Test]
        public void Check_converter()
        {
            object converter = null;

            TestDelegate statics = () => converter = TimeQuantityFixture.InternalConverter;

            Assert.That(statics, Throws.Nothing);
            Assert.That(converter, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the default constructor works correctly.
        /// </summary>
        [Test]
        public void Ctor_default()
        {
            new TimeQuantityFixture().Verify();
        }

        /// <summary>
        /// Tests that the <see cref="TimeUnit"/> constructor works correctly.
        /// </summary>
        /// <param name="unit"></param>
        [Test]
        [Combinatorial]
        public void Ctor_TimeUnit([TimeUnitValues] TimeUnit unit)
        {
            new TimeQuantityFixture(unit).Verify(unit);
        }

        /// <summary>
        /// Tests that the <see cref="double"/> constructor works correctly.
        /// </summary>
        /// <param name="value"></param>
        [Test]
        [Combinatorial]
        public void Ctor_double([TimeQuantityValues] double value)
        {
            new TimeQuantityFixture(value).Verify(value: value);
        }

        /// <summary>
        /// Tests that the <see cref="TimeUnit"/> and <see cref="double"/> constructor works correctly.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="value"></param>
        [Test]
        [Combinatorial]
        public void Ctor_TimeUnit_double([TimeUnitValues] TimeUnit unit, [TimeQuantityValues] double value)
        {
            new TimeQuantityFixture(unit, value).Verify(unit, value);
        }

        /// <summary>
        /// Verifies that the additive function works correctly.
        /// </summary>
        /// <param name="quantityAdder"></param>
        /// <param name="doubleAdder"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private void VerifyAdditive(Func<TimeQuantity, TimeQuantity, TimeQuantity> quantityAdder,
            Func<double, double, double> doubleAdder, TimeQuantity a, TimeQuantity b)
        {
            Assert.That(quantityAdder, Is.Not.Null);
            Assert.That(doubleAdder, Is.Not.Null);

            var result = quantityAdder(a, b);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Unit, Is.EqualTo(a.Unit));

            var aBase = ConvertToBase(a.Unit, a.Value);
            var bBase = ConvertToBase(b.Unit, b.Value);

            var expectedValue = ConvertFromBase(a.Unit, doubleAdder(aBase, bBase));
            Assert.That(result.Value, Is.EqualTo(expectedValue).Within(1e-3));
        }

        /// <summary>
        /// Tests that the addition operator works correctly.
        /// </summary>
        /// <param name="aUnit"></param>
        /// <param name="aValue"></param>
        /// <param name="bUnit"></param>
        /// <param name="bValue"></param>
        /// <see cref="VerifyAdditive"/>
        [Test]
        [Combinatorial]
        public void Addition_operator(
            [TimeUnitValues] TimeUnit aUnit, [TimeQuantityValues] double aValue,
            [TimeUnitValues] TimeUnit bUnit, [TimeQuantityValues] double bValue)
        {
            VerifyAdditive((a, b) => a + b, (a, b) => a + b,
                new TimeQuantity(aUnit, aValue),
                new TimeQuantity(bUnit, bValue));
        }

        /// <summary>
        /// Tests that the subtraction operator works correctly.
        /// </summary>
        /// <param name="aUnit"></param>
        /// <param name="aValue"></param>
        /// <param name="bUnit"></param>
        /// <param name="bValue"></param>
        /// <see cref="VerifyAdditive"/> 
        [Test]
        [Combinatorial]
        public void Subtraction_operator(
            [TimeUnitValues] TimeUnit aUnit, [TimeQuantityValues] double aValue,
            [TimeUnitValues] TimeUnit bUnit, [TimeQuantityValues] double bValue)
        {
            VerifyAdditive((a, b) => a - b, (a, b) => a - b,
                new TimeQuantity(aUnit, aValue),
                new TimeQuantity(bUnit, bValue));
        }

        /// <summary>
        /// Tests that the <see cref="TimeQuantity"/> and <see cref="double"/> multiplication operator works correctly.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="value"></param>
        /// <param name="factor"></param>
        [Test]
        [Combinatorial]
        public void Multiplication_operator_TimeQuantity_double([TimeUnitValues] TimeUnit unit,
            [TimeQuantityValues] double value, [TimeQuantityValues] double factor)
        {
            var quantity = new TimeQuantity(unit, value).Verify(unit, value);
            (quantity * factor).Verify(unit, value * factor);
        }

        /// <summary>
        /// Tests that the <see cref="double"/> and <see cref="TimeQuantity"/> multiplication operator works correctly.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="value"></param>
        /// <param name="factor"></param>
        [Test]
        [Combinatorial]
        public void Multiplication_operator_double_TimeQuantity([TimeQuantityValues] double factor,
            [TimeUnitValues] TimeUnit unit, [TimeQuantityValues] double value)
        {
            var quantity = new TimeQuantity(unit, value).Verify(unit, value);
            (factor*quantity).Verify(unit, factor*value);
        }

        /// <summary>
        /// Tests that the <see cref="TimeQuantity"/> division operator works correctly.
        /// </summary>
        /// <param name="aUnit"></param>
        /// <param name="aValue"></param>
        /// <param name="bUnit"></param>
        /// <param name="bValue"></param>
        [Test]
        [Combinatorial]
        public void Division_operator_TimeQuantities([TimeUnitValues] TimeUnit aUnit, [TimeQuantityValues] double aValue,
            [TimeUnitValues] TimeUnit bUnit, [TimeQuantityValues] double bValue)
        {
            Assert.That(bValue, Is.Not.EqualTo(0d));

            var a = new TimeQuantity(aUnit, aValue).Verify(aUnit, aValue);
            var b = new TimeQuantity(bUnit, bValue).Verify(bUnit, bValue);

            var actual = a/b;

            var aBase = ConvertToBase(aUnit, aValue);
            var bBase = ConvertToBase(bUnit, bValue);

            var expected = aBase/bBase;

            Assert.That(actual, Is.EqualTo(expected).Within(1e-3));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal static class TimeQuantityExtensionMethods
    {
        /// <summary>
        /// Performs basic verification on the <paramref name="quantity"/>.
        /// </summary>
        /// <typeparam name="TQuantity"></typeparam>
        /// <param name="quantity"></param>
        /// <param name="unit"></param>
        /// <param name="value"></param>
        /// <returns>The given <paramref name="quantity"/>.</returns>
        public static TQuantity Verify<TQuantity>(this TQuantity quantity,
            TimeUnit? unit = null, double value = default(double))
            where TQuantity : TimeQuantity
        {
            unit = unit ?? TimeQuantity.BaseUnit;
            Assert.That(quantity, Is.Not.Null);
            Assert.That(quantity.Unit, Is.EqualTo(unit.Value));
            Assert.That(quantity.Value, Is.EqualTo(value));
            return quantity;
        }
    }
}
