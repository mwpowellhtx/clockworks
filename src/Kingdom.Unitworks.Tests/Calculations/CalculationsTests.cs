using System.Linq;
using Kingdom.Unitworks.Dimensions;
using NUnit.Framework;

namespace Kingdom.Unitworks.Calculations
{
    using T = Dimensions.Systems.Commons.Time;
    using F = Dimensions.Systems.Commons.Frequency;
    using SiV = Dimensions.Systems.SI.Velocity;
    using SiA = Dimensions.Systems.SI.Area;
    using SiL = Dimensions.Systems.SI.Length;
    using UsL = Dimensions.Systems.US.Length;

    /// <summary>
    /// These calculations are by no means intended to represent anything one might find in
    /// any realistic physics book. Rather, these calculations are intended to exercise key
    /// aspects of the Quantity operators interacting well with each other and other quantities,
    /// that dimensions are handled correctly, and so on.
    /// </summary>
    public class CalculationsTests : TestFixtureBase
    {
        [Test]
        [TestCase(1e2, 1e2, 1e4)]
        public void Very_that_milliseconds_times_second_per_second_is_correct(double millisecondValue,
            double secondPerSecondValue, double expectedMillisecondValue)
        {
            var ms = T.Millisecond;
            var s = T.Second;

            var a = new Quantity(millisecondValue, ms);
            var b = new Quantity(secondPerSecondValue, s, s.Invert());

            // There is a lot that goes into a step like this one.
            var result = (a*b).ConvertTo(ms);

            Assert.That(result.Dimensions.AreCompatible(new[] {s}, true));

            Assert.That(result.Dimensions.OfType<ITime>().Single().Exponent, Is.EqualTo(1));

            Assert.That(result.Value, Is.EqualTo(expectedMillisecondValue).Within(1e-3));
        }

        [Test]
        [TestCase(1e2, 1e2, 1e4)]
        public void Very_that_milliseconds_times_second_hertz_is_correct(double millisecondValue,
            double secondHertzValue, double expectedMillisecondValue)
        {
            var ms = T.Millisecond;
            var s = T.Second;
            // ReSharper disable once InconsistentNaming
            var Hz = F.Hertz;

            var a = new Quantity(millisecondValue, ms);
            var b = new Quantity(secondHertzValue, s, Hz);

            // There is a lot that goes into a step like this one.
            var result = (a*b).ConvertTo(ms);

            Assert.That(result.Dimensions.AreCompatible(new[] {s}, true));

            Assert.That(result.Dimensions.OfType<ITime>().Single().Exponent, Is.EqualTo(1));

            Assert.That(result.Value, Is.EqualTo(expectedMillisecondValue).Within(1e-3));
        }

        [Test]
        [TestCase(100d, 200d, 66736)]
        public void Verify_that_ridiculous_dimension_times_absurd_dimension_is_correct_in_inches(
            double aValue, double bValue, int expectedValue)
        {
            var a = new Quantity(aValue,
                SiV.MetersPerMinute.Squared(),
                SiA.SquareMeter.Squared(),
                SiL.Kilometer.Invert());

            var b = new Quantity(bValue,
                SiV.MetersPerSecond,
                SiA.SquareMeter.Invert(),
                UsL.Foot.Invert());

            var s = T.Second;
            var inch = UsL.Inch;

            var initial = a*b;

            var result = initial.ConvertTo(inch);

            Assert.That((int) result.Value, Is.EqualTo(expectedValue));

            Assert.That(result.Dimensions
                .AreCompatible(new[] {inch.Cubed(), s.Invert().Cubed()}, true));
        }

        [Test]
        [TestCase(100d, 200d, 38)]
        public void Verify_that_ridiculous_dimension_times_absurd_dimension_is_correct_in_feet(
            double aValue, double bValue, int expectedValue)
        {
            var a = new Quantity(aValue,
                SiV.MetersPerMinute.Squared(),
                SiA.SquareMeter.Squared(),
                SiL.Kilometer.Invert());

            var b = new Quantity(bValue,
                SiV.MetersPerSecond,
                SiA.SquareMeter.Invert(),
                UsL.Foot.Invert());

            var s = T.Second;
            var ft = UsL.Foot;

            var initial = a*b;

            // Same as previous test but by several (four) orders of inch-to-foot magnitudes.
            var result = initial.ConvertTo(ft);

            Assert.That((int) result.Value, Is.EqualTo(expectedValue));

            Assert.That(result.Dimensions
                .AreCompatible(new[] {ft.Cubed(), s.Invert().Cubed()}, true));
        }
    }
}
