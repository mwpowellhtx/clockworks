using Kingdom.Unitworks.Dimensions;
using NUnit.Framework;

namespace Kingdom.Unitworks
{
    using L = Dimensions.Systems.SI.Length;
    using M = Dimensions.Systems.SI.Mass;

    public abstract class DimensionalQuantityMultiplicativeTestFixtureBase
        : DimensionalQuantityTestFixtureBase<Quantity, double>
    {
        //TODO: might do some math across a couple different units of measure among the same dimensions...
        protected IQuantity B { get; private set; }
        protected IQuantity C { get; private set; }
        protected IQuantity D { get; private set; }

        protected override void InitializeQuantities()
        {
            // For purposes of this test, does not matter what dimensions themselves are, per se.
            VerifyDimensionalQuantity(A = new Quantity(Value, L.Meter));
            VerifyDimensionalQuantity(B = new Quantity(Value, L.Meter));
            VerifyDimensionalQuantity(C = new Quantity(Value, M.Kilogram));
            VerifyDimensionalQuantity(D = new Quantity(Value, M.Kilogram));
        }

        protected override void VerifyResults(Quantity result, IQuantity a, IQuantity b, double expectedValue)
        {
            base.VerifyResults(result, a, b, expectedValue);

            CollectionAssert.IsNotEmpty(a.Dimensions);
            CollectionAssert.IsNotEmpty(b.Dimensions);

            var expectedDimensions = a.Dimensions.Multiply(b.Dimensions);
            Assert.That(result.Dimensions.AreEquivalent(expectedDimensions));

            Assert.That(result.Value, Is.EqualTo(expectedValue));
        }

        protected virtual void VerifyResults<TA, TB>(Quantity result, IQuantity a, IQuantity b, double expectedValue)
            where TA : IQuantity
            where TB : IQuantity
        {
            VerifyResults(result, (TA) a, (TB) b, expectedValue);
        }

        protected override void VerifyResults(Quantity result, IQuantity a, double b, double expectedValue)
        {
            base.VerifyResults(result, a, b, expectedValue);

            CollectionAssert.IsNotEmpty(result.Dimensions);
            CollectionAssert.IsNotEmpty(a.Dimensions);

            Assert.That(result.Dimensions.AreEquivalent(a.Dimensions));

            Assert.That(result.Value, Is.EqualTo(expectedValue));
        }
    }
}
