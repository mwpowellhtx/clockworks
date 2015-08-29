 using NUnit.Framework;

namespace Kingdom.Unitworks
{
    public abstract class DimensionlessQuantityBinaryOperatorTestFixtureBase<TResult, TExpected>
        : QuantityBinaryOperatorTestFixtureBase<TResult, TExpected>
    {
        [Test]
        public void Verify_Defaults()
        {
            // Nothing to do here but let the Setup fall through.
        }

        protected static void VerifyDimensionlessQuantity(IQuantity quantity, double expectedValue = Value)
        {
            Assert.That(quantity, Is.Not.Null);
            Assert.That(quantity.IsDimensionless, Is.True);
            Assert.That(quantity.Value, Is.EqualTo(expectedValue));

            Assert.That(quantity.Dimensions, Is.Not.Null);
            CollectionAssert.IsEmpty(quantity.Dimensions);
        }

        protected override void VerifyResults(Quantity result, IQuantity a, IQuantity b, double expectedValue)
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(a, Is.Not.Null);
            Assert.That(b, Is.Not.Null);

            Assert.That(result, Is.Not.SameAs(a));
            Assert.That(result, Is.Not.SameAs(b));

            Assert.That(result.IsDimensionless);
            Assert.That(a.IsDimensionless);
            Assert.That(b.IsDimensionless);

            Assert.That(result.Value, Is.EqualTo(expectedValue));
        }

        protected override void VerifyResults(Quantity result, IQuantity a, double b, double expectedValue)
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(a, Is.Not.Null);

            Assert.That(result, Is.Not.SameAs(a));

            Assert.That(result.IsDimensionless);
            Assert.That(a.IsDimensionless);

            Assert.That(result.Value, Is.EqualTo(expectedValue));
        }
    }
}
