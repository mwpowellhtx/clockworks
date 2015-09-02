using NUnit.Framework;

namespace Kingdom.Unitworks
{
    using A = Dimensions.Systems.SI.Area;
    using L = Dimensions.Systems.SI.Length;

    public class QuantityEquivalentTests : TestFixtureBase
    {
        [Test]
        public void Verify_area_equivalent()
        {
            var a = new Quantity(Value, A.SquareMeter);
            var b = new Quantity(Value, L.Meter.Squared());
            Assert.That(a.Equals(b), "{{{0}}} did not equal {{{1}}}.", a, b);
        }

        [Test]
        public void Verify_area_squared_equivalent()
        {
            var a = new Quantity(Value, A.SquareMeter.Squared());
            var b = new Quantity(Value, L.Meter.Squared().Squared());
            Assert.That(a.Equals(b), "{{{0}}} did not equal {{{1}}}.", a, b);
        }
    }
}
