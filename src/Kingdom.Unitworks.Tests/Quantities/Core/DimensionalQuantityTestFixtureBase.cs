using Kingdom.Unitworks.Dimensions;
using NUnit.Framework;

namespace Kingdom.Unitworks
{
    public abstract class DimensionalQuantityTestFixtureBase<TResult, TExpected>
        : QuantityBinaryOperatorTestFixtureBase<TResult, TExpected>
    {
        protected static void VerifyHomogeneousDimensions(IQuantity a, IQuantity b)
        {
            Assert.That(a.Dimensions.AreEquivalent(b.Dimensions));
        }

        protected static void VerifyHeterogeneousDimensions(IQuantity a, IQuantity b)
        {
            Assert.That(a.Dimensions.AreEquivalent(b.Dimensions), Is.False);
        }

        protected static void VerifyDimensionalQuantity(IQuantity quantity)
        {
            Assert.That(quantity, Is.Not.Null);
            Assert.That(quantity.IsDimensionless, Is.False);
            Assert.That(quantity.Value, Is.EqualTo(Value));

            Assert.That(quantity.Dimensions, Is.Not.Null);
            CollectionAssert.IsNotEmpty(quantity.Dimensions);
        }
    }
}
